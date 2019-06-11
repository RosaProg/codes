package com...util;


import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import javax.annotation.Resource;

import org.apache.commons.collections4.queue.CircularFifoQueue;
import org.apache.commons.io.FileUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

public class RInteractor {


	private Process p;
	private BufferedReader reader;
	InputStream stdout;
	private BufferedWriter writer;
	//TODO: find a way to see if this process exited or not
	private boolean processExited = false;
	private String text;
	private Long userId;
	private String cleanUsername;

	
	private Connector ;
	
	public RInteractor(Connector ,String username){
		this. = ;
		this.cleanUsername = username.replace(".", "_").replace("@", "_");
	}

	public String startCommandLineProcess(Long userId, String project,String imagePath,Boolean isPublic, Boolean isAdmin) throws IOException, InterruptedException {
		List<String> commands = new ArrayList<String>();

		
		if(!(isPublic && isAdmin)){
			commands.add("/usr/bin/sudo");
			commands.add("-u");
			commands.add(this.cleanUsername);				
		}
		

		//commands.add("-c");
		commands.add("/usr/bin/R");
		commands.add("--save");
		
		this.userId = userId;
		ProcessBuilder builder = new ProcessBuilder(commands);
//		builder.directory(new File(File.separator + "var" + File.separator + "lib" + File.separator + "" + File.separator + "-r-connector" + File.separator + userId.toString()));
		
		builder.redirectErrorStream(true);
		builder.redirectInput(ProcessBuilder.Redirect.PIPE);
		builder.redirectOutput(ProcessBuilder.Redirect.PIPE);
		builder.redirectError(ProcessBuilder.Redirect.PIPE);
		try {
			p = builder.start();
			processExited = false;

			stdout = p.getInputStream();
			OutputStream os = p.getOutputStream();
			writer = new BufferedWriter(new OutputStreamWriter(os));
			reader = new BufferedReader(new InputStreamReader(stdout));
			BufferedReader errorReader = new BufferedReader( new InputStreamReader(p.getErrorStream()));
			

			String response = convertStreamToStr();
			//String error = convertStreamToStr(errorReader);
			//System.out.println(error);
			text = response;
			//TODO: need to adapt this to work with the new folder it should no longer be in -r-connector
			
			String UploadFolder;
			if(isPublic){
				UploadFolder = .getUplopadFolder();
			}else{
				UploadFolder = .getUplopadFolder() + File.separator + userId.toString();
			}
			executeCommand(imagePath,"setwd(\"" + UploadFolder + File.separator + project +"\")",project,true,false,false,isPublic,isAdmin);
			executeCommand(imagePath,"options(error=dump.frames)",project,true,false,false,isPublic,isAdmin);
			
			if(!isPublic || (isPublic && isAdmin)){
				executeCommand(imagePath,"packrat::init()",project,true,false,false,isPublic,isAdmin);
					
			}
			
			
			//executeCommand("defaultDevice <- function(filename = \"r_image%03d.png\", ...) { \n png(filename, ...) \n } \n options(device = \"defaultDevice\") ",project,true,false,false);
			
			
			
			executeCommand(imagePath,"defaultDevice <- function(filename = \"r_image%03d.png\", ...) { \n png(filename, ...) \n } ",project,true,false,false,isPublic,isAdmin);
			executeCommand(imagePath,"options(device = \"defaultDevice\")",project,true,false,false,isPublic,isAdmin);
			executeCommand(imagePath,"local({r <- getOption(\"repos\")\n r[\"CRAN\"] <- \"http://cran.us.r-project.org\" \n options(repos=r) })",project,true,false,false,isPublic,isAdmin);
			
			
			
			if(new File(UploadFolder + File.separator + project + File.separator + ".RData").exists()){
				executeCommand(imagePath,"load(\".RData\")",project,true,false,false,isPublic,isAdmin);	
			}
			return response;
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return "failed to start";
		}
	
	}
	public Boolean isRAlive(){
		try{
			int exitValue = p.exitValue();	
			
		}catch (Exception e){
			return true;
		}
		return false;
	}
	
	public String executeCommand(String imagePath,String command,String project, boolean waitForResponse, boolean isPublic, boolean isAdmin) throws IOException, InterruptedException{
		return executeCommand(imagePath,command, project, waitForResponse,true,isPublic,isAdmin);
	}
	public String executeCommand(String imagePath,String command,String project, boolean waitForResponse,boolean allowStartup, boolean isPublic,boolean isAdmin)throws IOException, InterruptedException{
		return executeCommand(imagePath,command,project,waitForResponse,allowStartup,true,isPublic,isAdmin);
	}
	
	public String executeCommand(String imagePath, String command,String project, boolean waitForResponse,boolean allowStartup,boolean includeOutput, Boolean isPublic, boolean isAdmin)
			throws IOException, InterruptedException {
		//this checks to see if the process has already exited, if it has not it throws an exception 
		try{
			if(p == null){
				if(! allowStartup){
					return "";
				}
				text = startCommandLineProcess(userId,project,imagePath,isPublic,isAdmin);	
			}else{
				int exitValue = p.exitValue();	
				if(! allowStartup){
					return "";
				}
				text = startCommandLineProcess(userId,project,imagePath,isPublic,isAdmin);
			}
			
		}catch(IllegalThreadStateException e){
			//do nothing since the process is still running
		}
		String totalResponse = "";
		String[] commands = command.split("\n");
		for(int i = 0; i < commands.length; i++){
			String curCommand = commands[i];
			writer.write(curCommand + "\n");
			writer.flush();
			String response = convertStreamToStr(); 
			
		
			
			if(includeOutput){
				if(response.contains("Pango-WARNING")){
					response = curCommand;
				}
				String UploadFolder;
				
				if(isPublic){
					UploadFolder = .getUplopadFolder();
				}else{
					UploadFolder = .getUplopadFolder() + File.separator + userId.toString();
				}
				String filePath = UploadFolder + File.separator + project;
				
				
			File projectDirectory = new File(filePath);
				
				//get files and their timestamps
				File tempFile = new File(.getUplopadFolder() + File.separator + userId.toString() + File.separator + "~.temp");
				if(tempFile.exists()){
					tempFile.delete();
				}
				tempFile.createNewFile();
				
//				FileFilter filter = new AgeFileFilter(tempFile,false);
				
			
				File[] files = projectDirectory.listFiles();
				
				//so for now pray to christ we only have one new file
				boolean imagesFound = false;
				List<File> imageFiles = new ArrayList<File>();
				for(File file : files){
					
					if(!file.getName().equals(tempFile.getName()) && file.lastModified() >= tempFile.lastModified() && isImage(file.getName()) && file.getName().startsWith("r_image")){
						imageFiles.add(file);
						imagesFound = true;
					}
				}
				tempFile.delete();
				if(imagesFound){
					if(imageFiles.size() > 1 ){
						System.err.println("Multiple files found");
					}
						for(File file :imageFiles){
							if(imageFiles.size() > 1 ){
								System.err.println( file.getName() + " <-- file found in project " + project + "For user " );
							}      
							Long time = new Date().getTime();
							writer.write("dev.off()\n");
							writer.flush();
							File dest = new File(file.getParentFile().getCanonicalPath() + File.separator + time.toString() + ".png");
							FileUtils.moveFile(file, dest);

							
							String imageText = "\n <img onload=\"$('#r-console').scrollTop($('#r-console')[0].scrollHeight);\"src=\"" + imagePath + "/get/" + isPublic.toString() + "/" + project + "/" + dest.getName() + "\"/>\n";
							this.addText(imageText);
							response += imageText;							
						}
					
				}
				
				text +=   response + "\n";		
				
				
				
				totalResponse += response;
			}
		
		}
		
			
		return totalResponse;
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
	/*
	 * To convert the InputStream to String we use the Reader.read(char[]
	 * buffer) method. We iterate until the Reader return -1 which means there's
	 * no more data to read. We use the StringWriter class to produce the
	 * string.
	 */

	public String convertStreamToStr(BufferedReader reader) throws IOException {
		if(reader != null){
			StringBuilder sbMain = new StringBuilder();
			// reading characters instead of lines
			CircularFifoQueue<Character> queue = new CircularFifoQueue<Character>(3);
			Character c = null;
			boolean notBreak = false;

			while (notBreak) {
				int cInt = reader.read();
				c = (char) cInt;
				if ((c == null) || (cInt == -1))
					break;
				queue.offer(c);
				sbMain.append(c);
				// Circular buffer to find the last 2-3 letter
				StringBuilder sb = new StringBuilder();
				for (Character ch : queue) {
					sb.append(ch);
				}
				if (sb.toString().equalsIgnoreCase("\n> ") )
					notBreak = false;
			}

			System.out.print(sbMain.toString());
			String string = sbMain.toString();
			
			String[] splitStrings = string.split("\n");
			string = "";
			for(int i = 0; i < splitStrings.length; i++){
				if(! splitStrings[i].contains("setwd(")){
					string += splitStrings[i] + "\n";
				}
			}
			
			return string;	
		}else{
			return "";
		}
		
	}
	
	public String convertStreamToStr() throws IOException, InterruptedException {
		if(reader != null){
			StringBuilder sbMain = new StringBuilder();
			// reading characters instead of lines
			CircularFifoQueue<Character> queue = new CircularFifoQueue<Character>(3);
			Character c = null;
		//	Thread.sleep(50);
		//	System.out.println("Bytes avaliable to read" + new Integer(stdout.available()).toString());
			boolean notBreak = true;
			
			while (notBreak) {
				int cInt = reader.read();
				c = (char) cInt;
				if ((c == null) || (cInt == -1))
					break;
				queue.offer(c);
				sbMain.append(c);
				// Circular buffer to find the last 2-3 letter
				StringBuilder sb = new StringBuilder();
				for (Character ch : queue) {
					sb.append(ch);
				}
				
				
				if (sb.toString().equalsIgnoreCase("\n> ") || sb.toString().equalsIgnoreCase("\n+ ")){
						notBreak = false;
					
				}else{
					
				} 

			}

			//System.out.print(sbMain.toString());
			String string = sbMain.toString();
			
			String[] splitStrings = string.split("\n");
			string = "";
			for(int i = 0; i < splitStrings.length; i++){
				if(! splitStrings[i].contains("setwd(")){
					string += splitStrings[i] + "\n";
				}
			}
			
			return string;	
		}else{
			return "";
		}
		
	}

	public void interruptExecution(){
		if(p!= null){
			Integer pid = -1;
			if(p.getClass().getName().equals("java.lang.UNIXProcess")) {
			  /* get the PID on unix/linux systems */
			  try {
			    Field f = p.getClass().getDeclaredField("pid");
			    f.setAccessible(true);
			    pid = f.getInt(p);
				Runtime.getRuntime().exec("kill -SIGINT " + pid.toString());
								
			  } catch (Exception e) {
				  e.printStackTrace();
			  }
			}
		
		}
		

	}
	
	public String getText() {
		return text;
	}


	public void setText(String text) {
		this.text = text;
	}

	public void addText(String text) {
		this.text += text;
	}

}

class ProcMon implements Runnable {

	private final Process _proc;
	private volatile boolean _complete;

	public ProcMon(Process proc) {
		_proc = proc;
		// TODO Auto-generated constructor stub
	}

	public boolean isComplete() {
		return _complete;
	}

	public void run() {
		try {
			_proc.waitFor();
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		_complete = true;
	}

	public static ProcMon create(Process proc) {
		ProcMon procMon = new ProcMon(proc);
		Thread t = new Thread(procMon);
		t.start();
		return procMon;
	}
}