package com...web.controller;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileFilter;
import java.io.FileInputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.Charset;
import java.nio.file.FileSystems;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.nio.file.attribute.BasicFileAttributes;
import java.security.Principal;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collections;
import java.util.Date;
import java.util.HashMap;
import java.util.List;

import javax.annotation.PostConstruct;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

import org.apache.commons.io.FileUtils;
import org.apache.commons.io.IOUtils;
import org.apache.commons.io.filefilter.AgeFileFilter;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.multipart.MultipartFile;

import com...domain.Database;
import com...domain.Users;
import com...domain.web.form.ApiStatus;
import com...domain.web.form.ApiStatusEntity;
import com...domain.web.form.ApiStatusGeneric;
import com...service.UsersService;
import com...service.jpa.DatabaseService;
import com...util.Connector;
import com...util.Logger;
import com...util.PythonInteractor;
import com...util.RInteractor;
import com...util.ShellInteractor;

import static java.nio.file.StandardCopyOption.*;

/**
 * Handles requests for the application home page.
 */
@Controller
public class RController {

	private static final Logger logger = LoggerFactory.getLogger(RController.class);

	@Autowired
	Connector ;
	
	@Autowired
	DatabaseService dbService;
	
	@Autowired
	UsersService userService;
	
	@PersistenceContext
	EntityManager em;
	
	@Autowired
	Logger Logger;
	
	HashMap<Long,HashMap<String,RInteractor> > rInstances = new HashMap<Long,HashMap<String,RInteractor> >();
	HashMap<Long,HashMap<String,PythonInteractor> > pythonInstances = new HashMap<Long,HashMap<String,PythonInteractor> >();
	HashMap<Long,ShellInteractor>  shellInstances = new HashMap<Long,ShellInteractor> ();
	
	@PostConstruct
	public void start() throws Exception {
		.startServer();
		Runtime.getRuntime().exec("pkill R");			
		Runtime.getRuntime().exec("pkill python2");	
	}
	
	
	/**
	 * Initializes an R instance or returns the R text
	 * @param principal the user that is logged in
	 * @return
	 * @throws IOException
	 */
	@RequestMapping(value="/r/start",method=RequestMethod.POST)
	@ResponseBody
	public ApiStatusEntity<String> startR(@RequestParam("isPublic") Boolean isPublic,@RequestParam("imagePath") String imagePath,Principal principal,@RequestParam("project") String project, @RequestParam(required=false, defaultValue="false",value="force") Boolean force) throws IOException {
		
		ApiStatusEntity<String> response = new ApiStatusEntity<String>();
		try{
			Users user = userService.findByUsername(principal.getName());
			String initialOutput;
			//if user has not started an isntance yet start the instance and set the text to this original value
			RInteractor r;
			if((rInstances.get(user.getId()) == null) ||( rInstances.get(user.getId()).get(project) == null) || !rInstances.get(user.getId()).get(project).isRAlive() || force){
				if(force){
					rInterrupt(project,principal);
				}
				r = new RInteractor(,user.getUsername());
				if((rInstances.get(user.getId()) == null)){
					HashMap<String,RInteractor> tempMap = new HashMap<String,RInteractor>();
					rInstances.put(user.getId(), tempMap);
				}
				initialOutput = r.startCommandLineProcess(user.getId(),project,imagePath,isPublic,user.isAdmin());
				
	
				rInstances.get(user.getId()).put(project,r);
			}else{
				r = rInstances.get(user.getId()).get(project);
				initialOutput = r.getText();
			}
			
			response.setEntity(r.getText());
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
		}
	
		return response;
	}
	
	@RequestMapping(value="/r/command",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatusEntity<String> executeCommand(@RequestParam("isPublic") Boolean isPublic,@RequestParam("imagePath") String imagePath,@RequestParam(value="command") String cmd,@RequestParam(value="project") String project, Principal principal, @RequestParam(value="dbId",required=false) Long dbId) throws IOException {
		ApiStatusEntity<String> response = new ApiStatusEntity<String>();
		Users user = userService.findByUsername(principal.getName());
		Logger.logCommand(user.getUsername(), project, cmd, "r");
		
		if(cmd.startsWith("")){
			//format of request is
			// ("query","fileName")
			//
			if(dbId == null){
				response.setEntity("You need to select a database to run queries");
				response.setStatus("success");
			}else{
				Database db = dbService.findOne(dbId);
				String schemaName = db.getUser().getId().toString();
				String dbName = db.getName();
				int startPoint = cmd.indexOf("\"");
				int endPoint = cmd.indexOf("\"", startPoint + 1);
				
				String query = cmd.substring(startPoint + 1 , endPoint);
				startPoint = cmd.indexOf("\"",endPoint+1);
				endPoint = cmd.indexOf("\"",startPoint+1);
				
				String UploadFolder = .getUplopadFolder() + File.separator + user.getId();
				String fileName = UploadFolder  + File.separator + project + File.separator + cmd.substring(startPoint+1,endPoint);
				
				.sendMessage(query, schemaName, dbName,fileName);
				response.setEntity("File Created at " + fileName);
				response.setStatus("success");
			}
			
			
			
		}else{
			if(cmd.contains("setwd(") || cmd.contains("getwd(")){
				response.setErrorMsg("You cannot use the working directory commands in this sandboxed environment.");
				response.setStatus("fail");
				return response;
			}
			try{
				
				if(rInstances.get(user.getId()) == null){
					rInstances.put(user.getId(), new HashMap<String,RInteractor>());
				}
				RInteractor r = rInstances.get(user.getId()).get(project);
				if(r == null){
					startR(isPublic,imagePath,principal,project, false);	
				}
				
				String UploadFolder = .getUplopadFolder() + File.separator + user.getId();
				String content;
				String filePath;
				
					filePath = UploadFolder + File.separator + project;
					
				
				File projectDirectory = new File(filePath);
				//time stamp here
				
				
				String rResponse = r.executeCommand(imagePath,cmd,project, true,isPublic,user.isAdmin());
				
				response.setEntity(rResponse);
				response.setStatus("success");
			}catch(Exception e){
				e.printStackTrace();
				response.setStatus("fail");
				response.setErrorMsg("Error running command");
				
			}
		}
		
		
		return response;

	}
	
	
	@RequestMapping(value="/shell/start",method=RequestMethod.POST)
	@ResponseBody
	public ApiStatusEntity<String> startShell(Principal principal, @RequestParam(required=false, defaultValue="false",value="force") Boolean force) throws IOException {
		
		ApiStatusEntity<String> response = new ApiStatusEntity<String>();
		try{
			Users user = userService.findByUsername(principal.getName());
			String initialOutput;
			//if user has not started an isntance yet start the instance and set the text to this original value
			ShellInteractor shell;
			if((shellInstances.get(user.getId()) == null) || !shellInstances.get(user.getId()).isAlive() || force){
				shell = new ShellInteractor(,user.getUsername());
				
				shellInstances.put(user.getId(), shell);

				initialOutput = shell.startCommandLineProcess(user.getId());
				
	

			}else{
				shell = shellInstances.get(user.getId());
				initialOutput = shell.getText();
			}
			
			response.setEntity(shell.getText());
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
		}
	
		return response;
	}
	
	@RequestMapping(value="/shell/command",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatusEntity<String> executeCommandShell( String imagePath,@RequestParam(value="command") String cmd, Principal principal) throws IOException {
		ApiStatusEntity<String> response = new ApiStatusEntity<String>();
		Users user = userService.findByUsername(principal.getName());
		Logger.logCommand(user.getUsername(), cmd, "shell");
		
		try{
			
			ShellInteractor shell = shellInstances.get(user.getId());
			if(shell == null){
				startShell(principal, false);	
			}
			
			
			String rResponse = shell.executeCommand(cmd, true);
			
			response.setEntity(rResponse);
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Error running command");
			
		}
	
		
		return response;

	}
	
	
	static String readFile(String path, Charset encoding) 
	  throws IOException 
	{
	  byte[] encoded = Files.readAllBytes(Paths.get(path));
	  return new String(encoded, encoding);
	}


	@RequestMapping(value="/file/save",method=RequestMethod.POST)
	@ResponseBody
	ApiStatus saveFile(@RequestParam(value="isPublic",required=false,defaultValue="false") Boolean isPublicProject,@RequestParam(required=false,defaultValue="",value="project") String projectName,@RequestParam( value="name") String fileName,@RequestParam( value="newName") String fileNewName,@RequestParam( value="contents") String contents, Principal principal){
		ApiStatus response = new ApiStatus();
		
		try{
			fileName = fileName.replace(" ","_");
			fileNewName = fileNewName.replace(" ","_");
			Users user = userService.findByUsername(principal.getName());
			String UploadFolder;
			
			if(isPublicProject){
				if(user.isAdmin()){
					UploadFolder = .getUplopadFolder();
				}else{
					response.setStatus("fail");
					response.setErrorMsg("You do not have permissions to modify a public project");
					return response;
				}
			}else{
				UploadFolder = .getUplopadFolder() + File.separator + user.getId();
			}
			
			
			String filePath;
			if(projectName == ""){
				filePath = UploadFolder + File.separator + fileNewName;
				
			}else{
				filePath = UploadFolder + File.separator + projectName + File.separator + fileNewName;
					
			}
			File newFile = new File(filePath);
			
			if(! fileName.equals(fileNewName)){
				newFile.createNewFile();				
			}
			
			FileWriter fw = new FileWriter(newFile.getAbsoluteFile());
			BufferedWriter bw = new BufferedWriter(fw);
			bw.write(contents);
			bw.close();

			response.setStatus("success");			
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Unable to open the specified file.");
		}
		return response;
	}
	
	
	

	@RequestMapping(value="/file/rename",method=RequestMethod.POST)
	@ResponseBody
	ApiStatus renameFile(@RequestParam(value="isPublic",required=false,defaultValue="false") Boolean isPublicProject,@RequestParam(value="newName") String newFileName,@RequestParam(value="newProject") String newProjectName,@RequestParam(required=false,defaultValue="",value="project") String projectName,@RequestParam(required=false,defaultValue="", value="name") String fileName, Principal principal){
		ApiStatus response = new ApiStatus();
		
		try{
			newFileName = newFileName.replace(" ","_");
			Users user = userService.findByUsername(principal.getName());
			String UploadFolder;
			
			if(isPublicProject){
				if(user.isAdmin()){
					UploadFolder = .getUplopadFolder();
				}else{
					response.setStatus("fail");
					response.setErrorMsg("You do not have permissions to modify a public project");
					return response;
				}
			}else{
				UploadFolder = .getUplopadFolder() + File.separator + user.getId();
			}
			
			String filePath;
			String newFilePath;
			if(projectName.contains("..") || fileName.contains("..")){
				response.setErrorMsg("Intrusion attempt detected and logged.");
				response.setStatus("fail");
				return response;
			}
			if(projectName.equals("")){
				//its just a file with no project
				filePath = UploadFolder + File.separator + fileName;
			}else if (fileName.equals("")){
				//its a project
				filePath = UploadFolder + File.separator + projectName;
			}else{
				//its a file and a project
				filePath = UploadFolder + File.separator + projectName + File.separator + fileName;
			}

			if(newProjectName.equals("")){
				//its just a file with no project
				newFilePath = UploadFolder + File.separator + newFileName;
			}else if (fileName.equals("")){
				//its a project
				newFilePath = UploadFolder + File.separator + newProjectName;
			}else{
				//its a file and a project
				newFilePath = UploadFolder + File.separator + newProjectName + File.separator + newFileName;
			}
			
			
			
			Files.move(FileSystems.getDefault().getPath(filePath), FileSystems.getDefault().getPath(newFilePath), REPLACE_EXISTING);
			response.setStatus("success");
					
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Error deleting file.");
		}
		return response;
	}
	
	
	
	@RequestMapping(value="/file/delete",method=RequestMethod.POST)
	@ResponseBody
	ApiStatus deleteFile(@RequestParam(value="isPublic",required=false,defaultValue="false") Boolean isPublicProject,@RequestParam(required=false,defaultValue="",value="project") String projectName,@RequestParam(required=false,defaultValue="", value="name") String fileName, Principal principal){
		ApiStatus response = new ApiStatus();
		
		try{
			
			Users user = userService.findByUsername(principal.getName());
			
			String UploadFolder;
			
			if(isPublicProject){
				if(user.isAdmin()){
					UploadFolder = .getUplopadFolder();
				}else{
					response.setStatus("fail");
					response.setErrorMsg("You do not have permissions to modify a public project");
					return response;
				}
			}else{
				UploadFolder = .getUplopadFolder() + File.separator + user.getId();
			}
			
			String filePath;
			if(projectName.contains("..") || fileName.contains("..")){
				response.setErrorMsg("Intrusion attempt detected and logged.");
				response.setStatus("fail");
				return response;
			}
			if(projectName.equals("")){
				//its just a file with no project
				filePath = UploadFolder + File.separator + fileName;
			}else if (fileName.equals("")){
				//its a project
				filePath = UploadFolder + File.separator + projectName;
			}else{
				//its a file and a project
				filePath = UploadFolder + File.separator + projectName + File.separator + fileName;
			}
			
			File deleteFile = new File(filePath);
			if(deleteFile.delete()){
				response.setStatus("success");
			}else{
				response.setStatus("fail");
				response.setErrorMsg("Could not delete file.");
			}		
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Error deleting file.");
		}
		return response;
	}
	
	
	@RequestMapping(value="/file/create",method=RequestMethod.POST)
	@ResponseBody
	ApiStatus createFile(@RequestParam(value="isPublic") Boolean isPublicProject,@RequestParam(value="project") String projectName,@RequestParam( value="name") String fileName, Principal principal){
		ApiStatus response = new ApiStatus();
		
		try{
			fileName = fileName.replace(" ","_");
			Users user = userService.findByUsername(principal.getName());

			String UploadFolder;
			
			if(isPublicProject){
				if(user.isAdmin()){
					UploadFolder = .getUplopadFolder();
				}else{
					response.setStatus("fail");
					response.setErrorMsg("You do not have permissions to modify a public project");
					return response;
				}
			}else{
				UploadFolder = .getUplopadFolder() + File.separator + user.getId();
			}
			
			String filePath = UploadFolder + File.separator + projectName + File.separator + fileName;
			File newFile = new File(filePath);
			newFile.createNewFile();
			

			if(!isPublicProject){
				int exitCode = -1;
				String cleanUsername = user.getUsername().replace(".", "_").replace("@", "_");
				Process p;
				p = Runtime.getRuntime().exec("chown " + cleanUsername + " " + filePath);
				exitCode = p.waitFor();
				if(exitCode != 0){
					System.out.println("Code for chown user_uploads is = " + new Integer(exitCode).toString());
				}
				p = Runtime.getRuntime().exec("chmod -R 0700 " + filePath);
				exitCode = p.waitFor();
				if(exitCode != 0){
					System.out.println("Code for chmod user_uploads is = " + new Integer(exitCode).toString());
				}
					
			}
			
			
			
			response.setStatus("success");			
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Unable to open the specified file.");
		}
		return response;
	}
	
	private Boolean isImage(String fileName){
		if(fileName.endsWith(".png")){
			return true;
		}
		if(fileName.endsWith(".jpg")){
			return true;
		}
		if(fileName.endsWith(".jpeg")){
			return true;
		}
		if(fileName.endsWith(".pdf")){
			return true;
		}
		if(fileName.endsWith(".svg")){
			return true;
		}
		return false;
		
		
	}
	
	@RequestMapping(value="/project/create",method=RequestMethod.POST)
	@ResponseBody
	ApiStatus createProject(@RequestParam(value="isPublic",required=false,defaultValue="false") Boolean isPublicProject,@RequestParam("name") String name, Principal principal){
		Users user = userService.findByUsername(principal.getName());
		ApiStatus response = new ApiStatus();
		String UploadFolder;
		name = name.replace(" ","_");
		if(isPublicProject){
			if(user.isAdmin()){
				UploadFolder = .getUplopadFolder();
			}else{
				response.setStatus("fail");
				response.setErrorMsg("You do not have permissions to modify a public project");
				return response;
			}
		}else{
			UploadFolder = .getUplopadFolder() + File.separator + user.getId();
		}
		

		File projectFolder = new File(UploadFolder + File.separator + name);
		try{
			projectFolder.mkdirs();
			
			int exitCode = -1;
			String cleanUsername = user.getUsername().replace(".", "_").replace("@", "_");
			Process p;
			
			
			p = Runtime.getRuntime().exec("chown " + cleanUsername + " " + projectFolder);
			exitCode = p.waitFor();
			if(exitCode != 0){
				System.out.println("Code for chown user_uploads is = " + new Integer(exitCode).toString());
			}
			p = Runtime.getRuntime().exec("chmod -R 0700 " + projectFolder);
			exitCode = p.waitFor();
			if(exitCode != 0){
				System.out.println("Code for chmod user_uploads is = " + new Integer(exitCode).toString());
			}
			
			response.setStatus("success");
		}catch (Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("failed to create project");
			
		}
		return response;
	}
	
	@RequestMapping(value="/file/list",method=RequestMethod.POST)
	@ResponseBody
	ApiStatusGeneric<FileDescription> listFiles(@RequestParam(value="isPublic",required=false,defaultValue="false") Boolean isPublicProject,@RequestParam(required=false,value="isDirectory",defaultValue="false") Boolean isDirectory,@RequestParam(required=false,value="isImage",defaultValue="false") Boolean isImage,@RequestParam(required=false, value="project") String projectName, Principal principal){
		ApiStatusGeneric<FileDescription> response = new ApiStatusGeneric<FileDescription>();
		
		try{

			Users user = userService.findByUsername(principal.getName());
			String content;
			String filePath;
			
			String UploadFolder;
			if(isPublicProject){

					UploadFolder = .getUplopadFolder();

			}else{
				UploadFolder = .getUplopadFolder() + File.separator + user.getId();
			}
			
			if(projectName == null){
				filePath = UploadFolder;
					
			}else{
				filePath = UploadFolder + File.separator + projectName;
				
			}
			File directory = new File(filePath);
			List<FileDescription> files = new ArrayList<FileDescription>();
			List<FileDescription> extraFiles = new ArrayList<FileDescription>();
			
			for(File temp : directory.listFiles()){
				FileDescription fileDescription = new FileDescription();
				fileDescription.fileName = temp.getName();
				BasicFileAttributes attr = Files.readAttributes(Paths.get(temp.getAbsolutePath()), BasicFileAttributes.class);
				
				fileDescription.created = new Long(attr.creationTime().toMillis());
				fileDescription.fileSize = new Long(attr.size());
				fileDescription.modified = new Long(attr.lastModifiedTime().toMillis());
				fileDescription.isPublic = isPublicProject;
				if(isDirectory){
					if(temp.isDirectory()){
						if(isPublicProject){
							//dont show the upload folders in public projects which have numeric names
							if(! fileDescription.fileName.matches("-?\\d+(\\.\\d+)?")){
								fileDescription.isFolder = true;
								files.add(fileDescription);								
								
							}
						}else{
							fileDescription.isFolder = true;
							files.add(fileDescription);	
						}
						
					}else{
						extraFiles.add(fileDescription);
					}

				}else{
					if(isImage){
						if(isImage(fileDescription.fileName)){
							files.add(fileDescription);						
						}
					}else{
						if(!isImage(fileDescription.fileName) && !temp.isDirectory()){
							files.add(fileDescription);						
						}
		
					}					
				}

				
			}
			
			Collections.sort(files);
			Collections.sort(extraFiles);
			files.addAll(extraFiles);
			response.setList(files);
			response.setStatus("success");			
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Unable to open the specified file.");
		}
		return response;
	}
	
	/**
	 * gets the contest of a afile and returns them
	 * @param fileName name of the file to open
	 * @param projectName the project that the user currently has open
	 * @param principal information about the user
	 * @return the contents of the file
	 */
	@RequestMapping(value="/file/open",method=RequestMethod.POST)
	@ResponseBody
	ApiStatusEntity<String> getFile(@RequestParam(value="isPublic") Boolean isPublicProject,@RequestParam("name") String fileName,@RequestParam(required=false, value="project") String projectName, Principal principal){
		ApiStatusEntity<String> response = new ApiStatusEntity<String>();
		
		try{
			
			Users user = userService.findByUsername(principal.getName());

			String UploadFolder;
			if(isPublicProject){

					UploadFolder = .getUplopadFolder();
				
			}else{
				UploadFolder = .getUplopadFolder() + File.separator + user.getId();
			}
			
			String content;
			if(projectName == null){
				content = readFile(UploadFolder + File.separator + fileName , Charset.defaultCharset());
			}else{
				content = readFile(UploadFolder + File.separator + projectName + File.separator + fileName , Charset.defaultCharset());
			}
			response.setEntity(content);
			response.setStatus("success");			
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Unable to open the specified file.");
		}
		return response;
	}

	/**
	 * Initializes an R instance or returns the R text
	 * @param principal the user that is logged in
	 * @return
	 * @throws IOException
	 */
	@RequestMapping(value="/python/start",method=RequestMethod.POST)
	@ResponseBody
	public ApiStatusEntity<String> startPython(@RequestParam("isPublic") Boolean isPublic,Principal principal,@RequestParam("project") String project,@RequestParam(value="force",required=false, defaultValue="false") Boolean force) throws IOException {
		
		ApiStatusEntity<String> response = new ApiStatusEntity<String>();
		try{
			Users user = userService.findByUsername(principal.getName());
			String initialOutput;
			//if user has not started an isntance yet start the instance and set the text to this original value
			PythonInteractor r;
			if((pythonInstances.get(user.getId()) == null) || (pythonInstances.get(user.getId()).get(project) == null) || (!pythonInstances.get(user.getId()).get(project).isAlive())|| force){
				if(force){
					rInterrupt(project,principal);
				}
				r = new PythonInteractor(,user.getUsername());

				initialOutput = r.startCommandLineProcess(user.getId(),project,isPublic,user.isAdmin());
				if((pythonInstances.get(user.getId()) == null)){
					HashMap<String,PythonInteractor> tempMap = new HashMap<String,PythonInteractor>();
					pythonInstances.put(user.getId(), tempMap);
				}
	
				pythonInstances.get(user.getId()).put(project, r);
			}else{
				r = pythonInstances.get(user.getId()).get(project);
				initialOutput = r.getText();
			}
			
			response.setEntity(r.getText());
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
		}
	
		return response;
	}

	@RequestMapping(value="/r/interrupt",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatus rInterrupt(@RequestParam("project") String project, Principal principal){
		ApiStatus response = new ApiStatus();
		
		try{
		
			Users user = userService.findByUsername(principal.getName());
			RInteractor r= rInstances.get(user.getId()).get(project);
			r.interruptExecution();
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setStatus("Could not interrupt");
			
		}
		
		return response;
	}

	@RequestMapping(value="/shell/interrupt",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatus shellInterrupt(Principal principal){
		ApiStatus response = new ApiStatus();
		
		try{
		
			Users user = userService.findByUsername(principal.getName());
			ShellInteractor shell= shellInstances.get(user.getId());
			shell.interruptExecution();
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setStatus("Could not interrupt");
			
		}
		
		return response;
	}
	
	@RequestMapping(value="/python/interrupt",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatus pythonInterrupt(@RequestParam("project") String project, Principal principal){
		ApiStatus response = new ApiStatus();
		
		try{
		
			Users user = userService.findByUsername(principal.getName());
			PythonInteractor python = pythonInstances.get(user.getId()).get(project);
			python.interruptExecution();
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setStatus("Could not interrupt");
			
		}
		
		return response;
	}
	
	@RequestMapping(value="/python/add-package",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatus installPackage(@RequestParam("name") String packageName, @RequestParam Boolean isConda){
		ApiStatus response = new ApiStatus();
		   try{

			   Process p;
			   if(isConda){
				   p = Runtime.getRuntime().exec("/anaconda2/bin/conda install " + packageName + " -y -q");
				   	   
			   }else{
				   p = Runtime.getRuntime().exec("/anaconda2/bin/pip install " + packageName + " --allow-external " + packageName + " --allow-unverified " + packageName);
			
			   }
			   

			   	BufferedReader rd = new BufferedReader(new InputStreamReader(p.getInputStream()));
			   	BufferedReader rdError = new BufferedReader(new InputStreamReader(p.getErrorStream()));
			   	
			    String line;
			    boolean success = true;
			    String error = "";
			    while ((line = rd.readLine()) != null) {
			    	System.out.println(line);
			    	if(line.startsWith("Error:")){
			    		success = false;
			    	}
			    	error += line + "</br>";
			    }

			    
			   	int exitCode = p.waitFor();
			    if((exitCode == 0)&&(success)){
		    		response.setStatus("success");	
		    	}else{
			    	response.setStatus("fail");
			    	if(!error.equals("")){
			    		response.setErrorMsg(error);		    		
			    	}else{
			    		response.setErrorMsg("Could not install package");		    			
			    	}
			    	
		    	}
			   	
		    }catch (Exception e){
		    	e.printStackTrace();
		    	response.setStatus("fail");
		    	response.setErrorMsg("Could not install package");
		    }
		    return response;	
	}
	

	@RequestMapping(value="/python/list-installed-packages",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody	
	public ApiStatusGeneric<String> listInstalledPythonPackages(@RequestParam("isConda") Boolean isConda){
		ApiStatusGeneric<String> response = new ApiStatusGeneric<String>();
		
		   try{
			   Process p;
			   if(isConda){
				   p = Runtime.getRuntime().exec("/anaconda2/bin/conda list --no-pip");
				   
			   }else{
				   p = Runtime.getRuntime().exec("/anaconda2/bin/pip list ");
				      
			   }
			   BufferedReader stdInput = new BufferedReader(new 
					     InputStreamReader(p.getInputStream()));

				String s = null;
				response.setList(new ArrayList<String>());
				int count = 0;
				while ((s = stdInput.readLine()) != null) {
				    if(count > 2){
				    	String module = s.split(" ")[0];
						response.getList().add(module);
					    	
				    }
					count++;
				}
			   response.setStatus("success");
		    }catch (Exception e){
		    	e.printStackTrace();
		    	response.setStatus("fail");
		    	response.setErrorMsg("Could not list insalled packages");
		    }		
		
		return response;
	}
	
	@RequestMapping(value="/python/list-packages",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatusGeneric<String> listPythonPackages(@RequestParam("isConda") Boolean isConda) {
		ApiStatusGeneric<String> response = new ApiStatusGeneric<String>();
		List<String> packages = PythonInteractor.getAvailablePackages(isConda);
		if(packages.size() > 0){
			response.setList(packages);
			response.setStatus("success");
		 	
		}else{
			response.setErrorMsg("Could not retrieve Python Packages");
			response.setStatus("fail");
		}
		return response;
	}	
	@RequestMapping(value="/python/command",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatusEntity<String> executeCommandPython(HttpServletRequest request,@RequestParam("isPublic") Boolean isPublic,@RequestParam("project") String project,@RequestParam(value="command") String cmd, Principal principal, @RequestParam(value="dbId",required=false) Long dbId) throws IOException {
		ApiStatusEntity<String> response = new ApiStatusEntity<String>();
		Users user = userService.findByUsername(principal.getName());
		Logger.logCommand(user.getUsername(), project, cmd, "python");
		PythonInteractor r;		
		try{

			if(pythonInstances.get(user.getId()) == null){
				startPython(isPublic,principal,project,false);	
				r  = pythonInstances.get(user.getId()).get(project);
								
			}else{
				r  = pythonInstances.get(user.getId()).get(project);
				if(r == null){
					startPython(isPublic,principal,project,false);	
					r = pythonInstances.get(user.getId()).get(project);
				}	
			}
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Error running command");
			return response;
		}
		
		if(false){//cmd.startsWith("Query(")){
			//format of request is
			// ("query","fileName")
			//
			r.logCommand(cmd);
			if(dbId == null){
				response.setEntity("You need to select a database to run queries");
				response.setStatus("fail");
			}else{
				Database db = dbService.findOne(dbId);
				String schemaName = db.getUser().getId().toString();
				String dbName = db.getName();
				int startPoint = cmd.indexOf("\"");
				int endPoint = cmd.indexOf("\"", startPoint + 1);
				
				String query = cmd.substring(startPoint + 1 , endPoint);
				startPoint = cmd.indexOf("\"",endPoint+1);
				if(startPoint != -1){
					endPoint = cmd.indexOf("\"",startPoint+1);
					
					String UploadFolder = .getUplopadFolder() + File.separator + user.getId();
					String fileName = UploadFolder  + File.separator + project + File.separator + cmd.substring(startPoint+1,endPoint);
					
					.sendMessage(query, schemaName, dbName,fileName);
					response.setEntity("File Created at " + fileName);
					response.setStatus("success");	
				}else{
					response.setEntity(.sendMessage(query, schemaName, dbName,""));
					int truncateIndex = 0;
					  for (int i = 0; i < 3; i++) {
				            truncateIndex = response.getEntity().indexOf('\n', truncateIndex + 1);
			        }
				  response.setEntity(cmd + "\n" + response.getEntity().substring(truncateIndex,response.getEntity().length() ));
					
					response.setStatus("success");	
				}
				
			}
			
			
			
		}else{
		
			try{
				String url = request.getRequestURL().toString();
				url = url.substring(0,url.lastIndexOf("/"));
				url = url.substring(0,url.lastIndexOf("/"));
				url = url + "/database/query";
				String rResponse;
				if(dbId != null){
					rResponse = r.executeCommand(cmd,project, true,isPublic,user.isAdmin(),dbId,url);					
				}else{
					dbId = -1L;
					rResponse = r.executeCommand(cmd,project, true,isPublic,user.isAdmin(),dbId,url);
				}

				response.setEntity(rResponse);
				response.setStatus("success");
			}catch(Exception e){
				e.printStackTrace();
				response.setStatus("fail");
				response.setErrorMsg("Error running command");
				
			}
		}
		
		
		return response;

	}
	
	@RequestMapping("/image/get/{isPublic}/{project}/{image:.+}")
	public ResponseEntity<byte[]> getImage(@PathVariable Boolean isPublic,@PathVariable String project, @PathVariable String image,Principal principal) throws IOException {
		try{
			
		}catch (Exception e){
			e.printStackTrace();
		}
		Users user = userService.findByUsername(principal.getName());
		String UploadFolder;
		if(isPublic){
			UploadFolder = .getUplopadFolder();
		}else{
			UploadFolder = .getUplopadFolder() + File.separator + user.getId();
		}
		InputStream in = new FileInputStream(new File(UploadFolder + File.separator + project + File.separator + image));

	    final HttpHeaders headers = new HttpHeaders();
	    if(image.endsWith(".png")){
		    headers.setContentType(MediaType.IMAGE_PNG);	    	
	    }else if(image.endsWith(".jpg")||image.endsWith(".jpeg")){
		    headers.setContentType(MediaType.IMAGE_JPEG);	  
	    }else if(image.endsWith(".svg")){
		    headers.setContentType(MediaType.valueOf("image/svg+xml"));	  
	    }else if(image.endsWith(".pdf")){
	    	headers.setContentType(MediaType.valueOf("application/pdf"));	  
	    }


	    return new ResponseEntity<byte[]>(IOUtils.toByteArray(in), headers, HttpStatus.CREATED);
	}
	
	private class FileDescription implements Comparable<FileDescription>{
		public Long fileSize;
		public String fileName;
		public boolean isFolder = false;
		public Long created;
		public Long modified;
		public boolean isPublic = false;
		@Override
		public int compareTo(FileDescription o) {
			// TODO Auto-generated method stub
			return this.fileName.toLowerCase().compareTo(o.fileName.toLowerCase());
		}
	}
	
}

