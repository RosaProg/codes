package com...util;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;

import javax.annotation.PreDestroy;

public class Logger {

	private File commandLog;
	private File errorLog;
	FileWriter fw;
	BufferedWriter commandWriter;

	public Logger(){
		//create logging files if it does not exist and make sure we only can modify
		commandLog = new File("/disk1//command.log");
		if(! commandLog.exists()){
			try {
				commandLog.createNewFile();
				Process p;
				p = Runtime.getRuntime().exec("chown root /disk1//command.log");
				int exitCode = p.waitFor();
				if(exitCode != 0){
					System.out.println("Code for chown command.log was not 0");
				}
				p = Runtime.getRuntime().exec("chmod -R 0700 /disk1//command.log");
				exitCode = p.waitFor();
				if(exitCode != 0){
					System.out.println("Code for chmod command.log was not 0");
				}
				System.out.println("Created Command log");
				try {
					fw = new FileWriter(commandLog.getAbsoluteFile(),true);
					commandWriter = new BufferedWriter(fw);
					commandWriter.write("type|userame|project|command\n");
					commandWriter.flush();	
				} catch (IOException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}			
		
		
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch(InterruptedException e){
				e.printStackTrace();
			}
		}

	
	
try {
	fw = new FileWriter(commandLog.getAbsoluteFile(),true);
	commandWriter = new BufferedWriter(fw);

} catch (IOException e1) {
	// TODO Auto-generated catch block
	e1.printStackTrace();
}			

		errorLog = new File("/disk1//error.log");
		if(! errorLog.exists()){
			try {
				errorLog.createNewFile();
				System.out.println("Created Error log");
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		
	}
	

	public synchronized void logCommand(String username, String project, String command, String type){

		
		try {
			commandWriter.write(type + "|" + username + "" + "|" + project + "|" + command + "\n");
			
			commandWriter.flush();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	
	public synchronized void logCommand(String username, String command, String type){
		logCommand(username,"",command,type);
	}

	@PreDestroy
	public synchronized void shutDown(){
		try {
			commandWriter.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

}
