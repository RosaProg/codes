package com...web.controller;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.math.BigInteger;
import java.security.Principal;
import java.security.SecureRandom;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Scanner;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.servlet.http.HttpServletRequest;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
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
import com...service.jpa.EmailService;
import com...util.Connector;
import com...util.Logger;
import com...util.LogInteractor;
import com...util.Pbkdf2PasswordEncoder;

/**
 * Handles requests for the application home page.
 */
@Controller
public class DatabaseController {

	private static final Logger logger = LoggerFactory.getLogger(DatabaseController.class);

	@Autowired
	Connector ;
	
	@Autowired
	DatabaseService service;
	
	@Autowired
	UsersService userService;
	
	@PersistenceContext
	EntityManager em;
	
	@Autowired
	EmailService emailService;
	
	@Autowired
	Logger Logger;
	
	private HashMap<String,LogInteractor> logUtils = new HashMap<String,LogInteractor>();
	
	HashMap<Users,HashMap<String, File>> userUploads = new HashMap<Users,HashMap<String, File>>(); 
	String UploadBaseFolder = "";
	@RequestMapping(value="/database/upload",method={RequestMethod.POST})
	@ResponseBody
	public ApiStatus uploadFile(Principal principal,@RequestBody MultipartFile file, @RequestParam String name, @RequestParam(required=false, defaultValue="-1") int chunks, @RequestParam(required=false, defaultValue="-1") int chunk){
		ApiStatus response = new ApiStatus();
		Users user = userService.findByUsername(principal.getName());
		if(UploadBaseFolder == ""){
			UploadBaseFolder = .getUplopadFolder();
		}
		String UploadFolder = UploadBaseFolder + File.separator + user.getId();
		File folderUpload = new File(UploadFolder);
		
		if(folderUpload.exists()){
			if(chunk == 0){
				try {
					String filePath = UploadFolder + File.separator + name;
					file.transferTo(new File(filePath));
					
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
					
					
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				} 
			}else{
				FileOutputStream output = null;
				try {
					output = new FileOutputStream(UploadFolder + File.separator + name, true);
					try {
						output.write(file.getBytes());
					} catch (IOException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				} catch (FileNotFoundException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}finally{
					try {
						if(output != null){
							output.close();	
						}
						
					} catch (IOException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				}
				

			}	
		}else{
			//do something the upload folder doesnt exist
		}
		
	
		
		return response;
	}
	
	private class DatabaseSession{
		public String token;
		public String username;
		public String schemaName;
		public String databaseName;
		public HashMap<String,ResultSet> resultSetTokenToResultSet = new HashMap<String,ResultSet>();
		
	}
	private class ResultSet{
		private Pattern pattern;
		public Pattern columnPattern;
		public Matcher matcher;
		public Integer previousIndex = 0;
		public String result;
		public ResultSet(String result){
				this.result = result;
		       String regex = "\n(?=([^\']*[\'][^\']*[\'])*[^\']*$)";
		        pattern = Pattern.compile(regex);
		       
		        matcher = pattern.matcher(result);
		        String regex2 = "\\|(?=([^\']*[\'][^\']*[\'])*[^\']*$)";
		        columnPattern = Pattern.compile(regex2);
			       
		}

		public String token;
	}
	
	public class ResultResponse{
		public String status;
		public List<List<String> > rows = new ArrayList<List<String>>();
	}
	
	HashMap<String,DatabaseSession> tokenToSession = new HashMap<String,DatabaseSession>();
	
	@RequestMapping(value="/-jdbc/register",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public String openJDBCConnection(@RequestParam("username") String username, @RequestParam("password") String password){
	//	System.out.println("Username:" + username + "\n" + "Password:" + password);
	//	logger.info("test info");
		Users user = userService.findByUsername(username);
		if(user == null){
			return "fail";
		}
		Pbkdf2PasswordEncoder encoder = new Pbkdf2PasswordEncoder();
		if(encoder.isPasswordValid(user.getPassword(), password, username)){
			DatabaseSession session = new DatabaseSession();
			session.token = nextSessionId();
			session.username = username;
			session.schemaName = user.getId().toString();
			tokenToSession.put(session.token, session);
			return session.token;
		}else{
			return "fail";
		}
		
	}

	@RequestMapping(value="/-jdbc/query",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public String runQuery(@RequestParam("username") String username,@RequestParam("query") String query,@RequestParam("token") String token){
		try{
			
			DatabaseSession session = tokenToSession.get(token);
			String response;
			if(session.username.equals(username)){
				if(query.startsWith("use database")){
					String[] split = query.split(" ");
					session.databaseName = split[2];
					
					tokenToSession.get(token).databaseName = session.databaseName;
		//			return "using database " + session.databaseName;
				}
				response = .sendMessage(query, session.schemaName,session.databaseName,"");	
				if(response.startsWith("fail")){
					return "fail";
				}
				
					//create result set
					ResultSet resultSet = new ResultSet(response);
					
					resultSet.token = session.token;
					String resultSetToken = nextSessionId();
					session.resultSetTokenToResultSet.put(resultSetToken,resultSet);
					return resultSetToken;
				
			}else{
				return "fail";
			}
			
		}catch(Exception e){
			e.printStackTrace();
			return "fail";
		}
	}
	
	@RequestMapping(value="/database/servers-file",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatusGeneric<String> getServers(@RequestParam("fileLocation") String fileLocation, @RequestParam("pem") String pem, @RequestParam("numNodes") Integer numNodes){
		ApiStatusGeneric<String> response = new ApiStatusGeneric<String>();
		try{

			Scanner sc = new Scanner(new File(fileLocation));
			List<String> lines = new ArrayList<String>();
			while (sc.hasNextLine()) {
			  lines.add(sc.nextLine());
			}
			
			for(String line : lines){
				for(Integer numNode = 0; numNode < numNodes; numNode++){
					
					LogInteractor log;
					if(logUtils.containsKey(line + ":" + numNode.toString())){
						
					}else{
						log = new LogInteractor(pem,line,numNode);
						log.startCommandLineProcess();
						logUtils.put(line + ":" + numNode.toString(), log);		
					}
					
				}
				
			}
			
			response.setStatus("success");
			response.setList(lines);
		}catch(Exception e){
			response.setStatus("fail");
		}
		
		return response;
	}
	

	@RequestMapping(value="/database/get-log",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatusEntity<String> getLog(@RequestParam("ip") String ip, @RequestParam("nodeNum") String nodeNum, @RequestParam("isFirst") Boolean isFirst){
		ApiStatusEntity<String> response = new ApiStatusEntity<String>();
		try{
			if(! logUtils.containsKey(ip + ":" + nodeNum.toString())){
				response.setStatus("fail");
				response.setErrorMsg("Could not find this server configured. Did you send the right servers.config and pem files? for "  + ip);
				return response;
			}
			if(ip.equals("52.41.136.28")){
				int x = 0;
				x++;
			}
			response.setStatus("success");
			LogInteractor log = logUtils.get(ip + ":" + nodeNum.toString());
			if(isFirst){
				if(log.getText().equals("")){
					log.convertStreamToStr();
				}
				response.setEntity(log.getText());
			}else{
				response.setEntity(log.convertStreamToStr());				
			}

		}catch(Exception e){
			response.setStatus("fail");
			response.setErrorMsg("Error getting logging data for " + ip);
			
		}
		
		return response;
	}

	@RequestMapping(value="/-jdbc/get-results",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ResultResponse getResults(@RequestParam("token") String token,@RequestParam("resultSetToken") String resultSetToken){
		DatabaseSession session = tokenToSession.get(token);
		ResultSet results = session.resultSetTokenToResultSet.get(resultSetToken);
		ResultResponse response = new ResultResponse();
		if(results == null ){
			response.status = "fail";
			return response;
		}
		
		Matcher matcher = results.matcher;
		if(matcher.hitEnd()){
			response.status = "done";
			return response;
		}
		
		
		
		int rowCount = 0;
		while((rowCount < 10000) && matcher.find()){
			Matcher columnMatcher = results.columnPattern.matcher(results.result.substring(results.previousIndex,matcher.start()));
			List<String> row = new ArrayList<String>();
			int rowIndex = 0;
			while(columnMatcher.find()){
				row.add(results.result.substring(rowIndex + results.previousIndex ,results.previousIndex + columnMatcher.start()));
				rowIndex = columnMatcher.end();
			}
			row.add(results.result.substring(rowIndex + results.previousIndex ,matcher.start()));
			response.rows.add(row);
			results.previousIndex = matcher.end();
			rowCount++;
			
		}
		Matcher columnMatcher = results.columnPattern.matcher(results.result.substring(results.previousIndex,results.result.length()));
		List<String> row = new ArrayList<String>();
		int rowIndex = 0;
		while(columnMatcher.find()){
			row.add(results.result.substring(rowIndex + results.previousIndex ,results.previousIndex + columnMatcher.start()));
			rowIndex = columnMatcher.end();
		}
		row.add(results.result.substring(rowIndex + results.previousIndex ,results.result.length()));

		if(!(row.size() == 1 && row.get(0).equals(""))){
			response.rows.add(row);
			
		}
		if(!results.result.startsWith("time|")){
			response.status = "fail";
		}else{
			response.status = "success";	
		}
		
		return response;
	}
	
	private SecureRandom random = new SecureRandom();

	 public String nextSessionId() {
	   return new BigInteger(130, random).toString(32);
	 }
	  
	@RequestMapping(value="/database/query",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatusEntity<String> executeQuery(Principal principal,@RequestParam("query") String query,@RequestParam(required=false,value="dbId") Long dbId,@RequestParam(required=false, defaultValue="",value="fileName") String fileName,@RequestParam(required=false, defaultValue="",value="token") String token){
		ApiStatusEntity<String> response = new ApiStatusEntity<String>();
		Users user;
		Database db;
		if(token.equals("")){
			user = userService.findByUsername(principal.getName());
			Logger.logCommand(user.getUsername(), "", query, "");
			db = service.findOne(dbId);			
		}else{
			if(! .sessionExists(token)){
				response.setErrorMsg("Session does not exist");
				response.setStatus("fail");
				return response;
			}
			user = userService.findByUsername(.getSessionUsername(token));
			Logger.logCommand(user.getUsername(), "", query, "");
			db = service.findOne(.getSessionDatabase(token));

		}
		try{
			if(!db.getUsers().contains(user)){
				response.setStatus("fail");
				response.setErrorMsg("You don't have access to this database");
				return response;
			}
			String schemaName = db.getUser().getId().toString();
			String dbName = db.getName();
			if(fileName.equals("")){
				response.setEntity(.sendMessage(query, schemaName, dbName,""));	
			}else{
				
				if(UploadBaseFolder.equals("")){
					UploadBaseFolder = .getUplopadFolder();
				}
				String UploadFolder = UploadBaseFolder + File.separator + user.getId();
				String filePath = UploadFolder + File.separator + fileName;
				response.setEntity(.sendMessage(query, schemaName, dbName,filePath));
				
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
			response.setErrorMsg("We were unable to process your query.");
		}
		
		return response;
	}
	
	
	
	
	/**
	 * List out a  users databases
	 * @param principal - the databases this user has available to him or her
	 * @return
	 */
	@RequestMapping(value="/database/list",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatusGeneric<Database> list(Principal principal)
	{
		ApiStatusGeneric<Database> response = new ApiStatusGeneric<Database>();
		
		try{
			
			
			List<Database> databases = userService.findByUsername(principal.getName()).getDatabases();
			
			response.setList(new ArrayList<Database>());
			for(Database database : databases){
				List<Users> users = new ArrayList<Users>();
				for(Users user : database.getUsers()){
					Users temp = new Users();
					temp.setId(user.getId());
					
					
				}
				em.detach(database);

				database.setUsers(users);
				database.getUser().setFriends(null);
				database.getUser().setDatabases(null);
				database.setOwned( principal.getName().equals(database.getUser().getUsername()));
				if(!response.getList().contains(database)){
					response.getList().add(database);
				}
			}
			response.setStatus("success");
			
		}catch(Exception e){
			e.printStackTrace();
			response.setErrorMsg("We are experiencing some technical difficulties please try again in a moment.");
			response.setStatus("fail");
		}
		return response;
	}
	
	@RequestMapping(value="/database/save",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatusEntity<Database> save(@RequestParam("name") String name,@RequestParam("id") Long id,Principal principal,HttpServletRequest request)
	{
		ApiStatusEntity<Database> response = new ApiStatusEntity<Database>();
		
		try{
			name = name.replace(" ","_");
			Database database = new Database();
			database.setName(name);

			if(id == -1){
				database.setId(null);
				database.setUser(userService.findByUsername(principal.getName()));				
				 if(.sendMessage("create database " + name, database.getUser().getId().toString()).contains("database created")){
					 //ok  created it
				 }else{
					 response.setErrorMsg("Could not create database.");
						response.setStatus("fail"); 
						return response;
				 }
			}else{
				database = service.findOne(id);
				
			}

			
			database = service.save(database);
			addUserToDB(principal,principal.getName(),database.getId(),request);
			
			response.setEntity(database);
			em.detach(database);
			database.setUsers(null);
			database.getUser().setFriends(null);
			database.getUser().setDatabases(null);
			if(name.contains(" ")){
				response.setErrorMsg("Names cannot contain spaces.");
				response.setStatus("fail");
				return response;
			}

			response.setStatus("success");
			
		}catch(org.springframework.dao.DataIntegrityViolationException e){
			e.printStackTrace();
			response.setErrorMsg("You already have a database with this specified name.");
			response.setStatus("fail");
			
		}catch(Exception e){
			e.printStackTrace();
			response.setErrorMsg("We are experiencing some technical difficulties please try again in a moment.");
			response.setStatus("fail");
		}
		return response;
	}

	@RequestMapping(value="/user/removeFromDb")
	@ResponseBody
	public ApiStatus removeUserFromDB(Principal principal,@RequestParam("username") String username,@RequestParam("dbId") Long dbId){
		
		ApiStatus response = new ApiStatus();
		
		Database db = service.findOne(dbId);
		
		if(! db.getUser().equals(userService.findByUsername(principal.getName())) ){
			response.setStatus("fail");
			response.setErrorMsg("You do not have the permissions necessary to add users to this database.");
			return response;
		}
		

	    try {

			Users user = userService.findByUsername(username);
			for(int i = 0; i < db.getUsers().size(); i++){
				if(db.getUsers().get(i).equals(user)){
					db.getUsers().remove(i);
				}
			}

			service.save(db);
	        response.setStatus("success");
	        	
	    } catch (Exception e) {
	        e.printStackTrace();
	        response.setStatus("fail");
	    }
		
		return response;
	}
	
	@RequestMapping(value="/user/addToDb")
	@ResponseBody
	public ApiStatus addUserToDB(Principal principal,@RequestParam("username") String username,@RequestParam("dbId") Long dbId,HttpServletRequest request){
		
		ApiStatus response = new ApiStatus();
		
		Database db = service.findOne(dbId);
		
		if(! db.getUser().equals(userService.findByUsername(principal.getName())) ){
			response.setStatus("fail");
			response.setErrorMsg("You do not have the permissions necessary to add users to this database.");
			return response;
		}
		

	    try {

			Users user = userService.findByUsername(username);
			if(user == null){
				//add the user and send out invite
				user = new Users();
				user.setUsername(username);
				user = userService.save(user);				
				String url = request.getRequestURL().toString();
				String baseURL = url.substring(0, url.length() - request.getRequestURI().length()) + request.getContextPath() + "/";

//				emailService.sendEmail(username, "Someone has shared a datastore with you on <a href=\"" + baseURL +"/views/register"+ "\"> DB", "");
				response.setStatus("fail");
				response.setErrorMsg("User does not have an account!");
			}else{
				db.getUsers().add(user);


				service.save(db);
		        response.setStatus("success");
		        				
			}
			
	
	    } catch (Exception e) {
	        e.printStackTrace();
	        response.setStatus("fail");
	    }
		
		return response;
	}
	
	@RequestMapping(value="/database/delete",method={RequestMethod.POST,RequestMethod.GET})
	@ResponseBody
	public ApiStatus delete(Long id){
		ApiStatus response = new ApiStatus();
		
		try{

			Database database =service.findOne(id);
			service.delete(database);
			
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("We are experiencing some technical difficulties please try again in a moment.");

		}
		
		return response;
		
	}

	
		
	
}

