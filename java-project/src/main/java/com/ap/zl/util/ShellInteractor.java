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

import org.apache.commons.collections4.queue.CircularFifoQueue;
import org.apache.commons.io.FileUtils;
import org.springframework.beans.factory.annotation.Autowired;

public class ShellInteractor {


	
	
	private Process p;
	InputStream stdout;
	private BufferedReader reader;
	private BufferedReader errorReader;
	

	
	private BufferedWriter writer;
	//TODO: find a way to see if this process exited or not
	private boolean processExited = false;
	private String text;
	private Long userId;
	private Connector ;
	private String cleanUsername;
	public ShellInteractor(Connector ,String username) {
		this. = ;
		this.cleanUsername = username.replace(".", "_").replace("@", "_");
	}


	public String startCommandLineProcess(Long userId) throws IOException {
		List<String> commands = new ArrayList<String>();
		//commands.add("/bin/su");
		//commands.add("/usr/bin/sudo");
		//commands.add("-u");
		//commands.add(this.cleanUsername);
		//commands.add("-c");
	//	commands.add("sh");
		commands.add("/usr/bin/sudo");
		commands.add("-u");
		commands.add(this.cleanUsername);
		//commands.add("-c");
		commands.add("/bin/sh");
		commands.add("-i");
		//	commands.add("-c");
	//	commands.add("print 42");

		
		this.userId = userId;
		ProcessBuilder builder = new ProcessBuilder(commands);
		String ProjectFolder = .getUplopadFolder() + File.separator + userId.toString();
		
		builder.directory(new File(ProjectFolder));
		builder.redirectErrorStream(true);
		builder.redirectInput(ProcessBuilder.Redirect.PIPE);
		builder.redirectOutput(ProcessBuilder.Redirect.PIPE);
		builder.redirectError(ProcessBuilder.Redirect.PIPE);
		try {

			p = builder.start();
			stdout = p.getInputStream();
			OutputStream os = p.getOutputStream();
			InputStream errorStream = p.getErrorStream();
			
			processExited = false;			
			reader = new BufferedReader(new InputStreamReader(stdout));
			errorReader= new BufferedReader(new InputStreamReader(errorStream));
			
			
			writer = new BufferedWriter(new OutputStreamWriter(os));
			
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		String response = convertStreamToStr();
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

	public String executeCommand(String command,boolean waitForResponse)
			throws IOException {
		return executeCommand(command,waitForResponse,true);
	}
	
	public String executeCommand(String command, boolean waitForResponse, boolean isStartup)
			throws IOException {
		//this checks to see if the process has already exited, if it has not it throws an exception 
		try{
			if(p == null){
				if(! isStartup ){
					return "";
				}
				text = startCommandLineProcess(userId);	
			}else{
				int exitValue = p.exitValue();	
				if(! isStartup ){
					return "";
				}
				text = startCommandLineProcess(userId);
			}
			
		}catch(IllegalThreadStateException e){
			//do nothing since the process is still running
		}
		
		String totalResponse = "";
		String[] commands = command.split("\n",-1);

		for(int i = 0; i < commands.length; i++){
			String curCommand = commands[i];
			writer.write(curCommand + "\n");
			writer.flush();
			String response = convertStreamToStr(); 
			
			

			text +=   response + "\n";		
			
			
			
			totalResponse += response;
		}
		

		text += "$" + command + "\n" + totalResponse;

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
		CircularFifoQueue<Character> queue = new CircularFifoQueue<Character>(2);
		Character c = null;
		boolean notBreak = true;

		while (notBreak) {

			c = (char) reader.read();

			System.out.print(c);
			if ((c == null) || (c == '\uFFFF'))
				break;
			queue.offer(c);
			sbMain.append(c);
			// Circular buffer to find the last 2-3 letter
			StringBuilder sb = new StringBuilder();
			for (Character ch : queue) {
				sb.append(ch);
			}
			if (stdout.available() == 0 && (sb.toString().equalsIgnoreCase("\n$") || sb.toString().equalsIgnoreCase(" $")) )
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

}

