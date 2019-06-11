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

public class LogInteractor {


	
	
	private Process p;
	private InputStream stdout;
//	private InputStream reader;
	private BufferedReader errorReader;
	

	

	//TODO: find a way to see if this process exited or not
	private boolean processExited = false;
	private String text;
	private String pemLocation;
	private String ip;
	private Integer numNode;
	public LogInteractor(String pemLocation,String ip, Integer numNode) {
		this.pemLocation = pemLocation;
		this.ip = ip;
		this.numNode = numNode;
		this.text = "";
	}


	public String startCommandLineProcess() throws IOException {
		List<String> commands = new ArrayList<String>();

		commands.add("ssh");
		commands.add("-oStrictHostKeyChecking=no");
		commands.add("-i");
		commands.add(this.pemLocation);
		commands.add("ubuntu@"+ip);
		if(this.numNode.equals(0)){
			//commands.add("\\\"ls -h\\\"");
			commands.add("tail");
			commands.add("-f");
			commands.add("/var/log/upstart/.log");
//			commands.add("\"tail -f /var/log/upstart/.log\"");
	//		commands.add("\""");
				
		}else{
			Integer node = this.numNode + 1;
			commands.add("tail");
			commands.add("-f");
			commands.add("/var/log/upstart/" + node.toString() + ".log");
			
		}
		

		ProcessBuilder builder = new ProcessBuilder(commands);
		

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
		//	reader = new BufferedReader(new InputStreamReader(stdout));
			errorReader= new BufferedReader(new InputStreamReader(errorStream));
			
			
		//	writer = new BufferedWriter(new OutputStreamWriter(os));
			
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	//	String response = convertStreamToStr();
	//	text = response;
		return "";
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
	
		byte[] tmp = new byte[1024];
		while (stdout.available() > 0) {
		     int i = stdout.read(tmp, 0, 1024);
		     if (i < 0)
		          break;
		     sbMain.append(new String(tmp, 0, i));
		}
		/*
		int numRead = 0;
		while (numRead )
		stdout.r
		String line = reader.readLine();
		String output = "";
		while(line != null){
			output += line;
			if(stdout.available() != 0){
				line = reader.readLine();
	
			}else{
				line = null;
			}

		}

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
			if (stdout.available() == 0)
				notBreak = false;
		}
		
*/
	//	System.out.print(sbMain.toString());
		String string = sbMain.toString();
		this.text += string;
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

