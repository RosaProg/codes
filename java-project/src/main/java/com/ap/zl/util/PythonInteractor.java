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
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.apache.commons.collections4.queue.CircularFifoQueue;
import org.apache.commons.io.FileUtils;
import org.springframework.beans.factory.annotation.Autowired;

public class PythonInteractor {



	private Process p;
	InputStream stdout;
	private BufferedReader reader;
	private BufferedWriter writer;
	//TODO: find a way to see if this process exited or not
	private boolean processExited = false;
	private String text;
	private Long userId;
	private Connector ;
	private String cleanUsername;
	private String username;
	List<String> pipPackages = new ArrayList<String>();
	
	public PythonInteractor(Connector ,String username) {
		this. = ;
		this.username = username;
		this.cleanUsername = username.replace(".", "_").replace("@", "_");
	
	}

	
	private static final Pattern TAG_REGEX = Pattern.compile("<a.+?>(.+?)</a>");

	private static List<String> getTagValues(final String str) {
	    final List<String> tagValues = new ArrayList<String>();
	    final Matcher matcher = TAG_REGEX.matcher(str);
	    while (matcher.find()) {
	        tagValues.add(matcher.group(1));
	    }
	    return tagValues;
	}
	
	public static List<String> getAvailablePackages(Boolean isConda){
		List<String> packages = new ArrayList<String>();

		List<String> commands = new ArrayList<String>();

		//commands.add("-c");
		
		if(isConda){
			commands.add("/anaconda2/bin/conda");
			commands.add("search");
			commands.add("--names-only");

			ProcessBuilder builder = new ProcessBuilder(commands);
		
			builder.redirectInput(ProcessBuilder.Redirect.PIPE);
			builder.redirectOutput(ProcessBuilder.Redirect.PIPE);
			try {
				Process p = builder.start();
				
				InputStream stdout = p.getInputStream();
				BufferedReader reader = new BufferedReader(new InputStreamReader(stdout));
				
				p.waitFor();
				int count  = 0;
				String line;
				while((line = reader.readLine()) != null){
					if(count > 1){
						packages.add(line);
					}
					count++;
				}
			} catch (Exception e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
	
		}else{
			String urlToRead = "https://pypi.python.org/simple/";
		    StringBuilder result = new StringBuilder();
		    try{
		    	URL url = new URL(urlToRead);
			    HttpURLConnection conn = (HttpURLConnection) url.openConnection();
			    conn.setRequestMethod("GET");
			    BufferedReader rd = new BufferedReader(new InputStreamReader(conn.getInputStream()));
			    String line;
			    while ((line = rd.readLine()) != null) {
			       result.append(line);
			    }
			    rd.close();
			    
			    String text = result.toString();
			    packages = getTagValues(text);
		    }catch (Exception e){
		    	e.printStackTrace();

		    }
		}
		
		return packages;
	}

	public String startCommandLineProcess(Long userId,String project,Boolean isPublic, Boolean isAdmin) throws IOException {
		List<String> commands = new ArrayList<String>();
		//commands.add("/bin/su");
		if(!(isPublic && isAdmin)){
			commands.add("/usr/bin/sudo");
			commands.add("-u");
			commands.add(this.cleanUsername);				
		}
		
		//commands.add("-c");
		commands.add("/anaconda2/bin/python2");
		commands.add("-i");
		//	commands.add("-c");
	//	commands.add("print 42");

		
		this.userId = userId;
		ProcessBuilder builder = new ProcessBuilder(commands);
		String UploadFolder;
		if(isPublic){
			UploadFolder = .getUplopadFolder() + File.separator + project;
		}else{
			UploadFolder = .getUplopadFolder() + File.separator + userId.toString() + File.separator + project;
		}
		
		builder.directory(new File(UploadFolder));
		builder.redirectErrorStream(true);
		builder.redirectInput(ProcessBuilder.Redirect.PIPE);
		builder.redirectOutput(ProcessBuilder.Redirect.PIPE);
		builder.redirectError(ProcessBuilder.Redirect.PIPE);
		try {
			p = builder.start();
			processExited = false;
			
			stdout = p.getInputStream();
			OutputStream os = p.getOutputStream();

			reader = new BufferedReader(new InputStreamReader(stdout));
			writer = new BufferedWriter(new OutputStreamWriter(os));
			
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		String response = convertStreamToStr();
		
		String Function = "import requests\ndef Query(url,token,query,fileName):\n	r = requests.post(url, data={'token': token, 'query': query, 'fileName': fileName})\n	if r.text.startswith('<html>'):\n		print 'error running query'\n	else:\n		print r.text\n";
		this.executeCommand(Function, project, false, false, isPublic, isAdmin, -1L,"");
		text = response;
		return response;
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

	public String executeCommand(String command,String project, boolean waitForResponse,boolean isPublic, boolean isAdmin,Long dbId,String url)
			throws IOException {
		return executeCommand(command,project,waitForResponse,true,isPublic,isAdmin,dbId,url);
	}
	
	public String executeCommand(String command,String project, boolean waitForResponse, boolean isStartup,boolean isPublic, boolean isAdmin, Long dbId,String url)
			throws IOException {
		//this checks to see if the process has already exited, if it has not it throws an exception 
		try{
			if(p == null){
				if(! isStartup ){
					return "";
				}
				text = startCommandLineProcess(userId,project,isPublic,isAdmin);	
			}else{
				int exitValue = p.exitValue();	
				if(! isStartup ){
					return "";
				}
				text = startCommandLineProcess(userId,project,isPublic,isAdmin);
			}
			
		}catch(IllegalThreadStateException e){
			//do nothing since the process is still running
		}
		
		
			/*	
		writer.write(command + "\n");
		writer.flush();
		String totalResponse = convertStreamToStr(); 
		*/	
		String token = "";
		if(!dbId.equals("-1")){
			token = .createSession(this.username, this.userId.toString(), dbId);
				
		}
		
		String totalResponse = "";
		//TODO: This needs to be fixed so that they can have linebreaks some day
		String[] commands = command.split("\n",-1);
		String response = "";
		for(int i = 0; i < commands.length; i++){
			String curCommand = commands[i];
			if(curCommand.replaceAll("\\s+","") .contains("Query") && ! curCommand.startsWith("def Query")){
				if(dbId.equals(-1)){
					response = "You must have a database selected to run a Query";
					return response;
				}else{
					
					int startPoint = curCommand.indexOf("\"");
					int endPoint = curCommand.indexOf("\"", startPoint + 1);
					

					startPoint = curCommand.indexOf("\"",endPoint+1);
					if(startPoint == -1){
						curCommand = curCommand.replace(")", ",'')");
						//curcommand is Query("something","someting")
						curCommand = curCommand.replace("Query(", "Query('" + url + "','" + token +"'," );
					
					}
				}
			}
			writer.write(curCommand + "\n");
			writer.flush();
			response = convertStreamToStr(); 
			if(response.equals(" >>>")){
				response = ">>>";
			}
			if(response.equals(" ...")){
				response = "...";
			}
								
				totalResponse +=  " " + curCommand + "\n" +  response;				
				

			
			
			//totalResponse = totalResponse.replace(" ...", "");
			text += totalResponse;
		}
			.removeSession(token);
		return totalResponse;
	}

	/*
	 * To convert the InputStream to String we use the Reader.read(char[]
	 * buffer) method. We iterate until the Reader return -1 which means there's
	 * no more data to read. We use the StringWriter class to produce the
	 * string.
	 */

	public String convertStreamToStr() throws IOException {

		StringBuilder sbMain = new StringBuilder();
		// reading characters instead of lines
		CircularFifoQueue<Character> queue = new CircularFifoQueue<Character>(4);
		Character c = null;
		boolean notBreak = true;

		while (notBreak) {
			c = (char) reader.read();
			System.out.print(c);
			if (c == null)
				break;
			queue.offer(c);
			sbMain.append(c);
			// Circular buffer to find the last 2-3 letter
			StringBuilder sb = new StringBuilder();
			for (Character ch : queue) {
				sb.append(ch);
			}
			if (stdout.available() == 0 &&(sb.toString().equalsIgnoreCase("\n>>>") || sb.toString().equalsIgnoreCase(" >>>")||sb.toString().equalsIgnoreCase("\n...") || sb.toString().equalsIgnoreCase(" ...")))
				notBreak = false;
		}

	//	System.out.print(sbMain.toString());
		String string = sbMain.toString();

		return string;
	}


	public String getText() {
		return text;
	}


	public void setText(String text) {
		this.text = text;
	}


	public boolean isAlive() {
		try{
			int exitCode = p.exitValue();
		}catch(Exception e){
			return true;
		}
		return false;
	}

	public void logCommand(String cmd) {
		text += cmd + "\n";
	}

}

