package com...util;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.io.Writer;
import java.math.BigInteger;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.Socket;
import java.net.URL;
import java.net.URLEncoder;
import java.net.UnknownHostException;
import java.nio.charset.StandardCharsets;
import java.security.SecureRandom;
import java.util.HashMap;

import javax.annotation.PostConstruct;

import org.apache.commons.net.ftp.FTP;
import org.apache.commons.net.ftp.FTPClient;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;


/**
 * Well this feels absurdly simple... this is how we connect to , its very rudimentary since it basically gives the server
 * carte blanche on what it can do in , we may want to change that at some point
 * @author Felipe
 *
 */

public class Connector {
	private final Integer port; //the port you are running  on
	private final String host; //the address of the host you are running  on
	private final Integer launcherPort; //the port you are running  on
	
	private String uploadFolder = "";
	
	private class DatabaseSession{
		public String username;
		public String schemaName;		
		public Long dbId;

	}
	
	private HashMap<String,DatabaseSession> tokenToSession = new HashMap<String,DatabaseSession>();
private final String USER_AGENT = "Mozilla/5.0";
private SecureRandom random = new SecureRandom();
	// HTTP POST request

	public String createSession(String username,String schemaName, Long dbId){
		String token = new BigInteger(130, random).toString(32);
		DatabaseSession session = new DatabaseSession();
		session.username = username;
		session.schemaName = schemaName;
		session.dbId = dbId;
		tokenToSession.put(token, session);
		return token;
	}
	
	public boolean sessionExists(String token){
		return tokenToSession.get(token) != null;
	}
	
	public String getSessionUsername(String token){
		return tokenToSession.get(token).username;
	}
	public String getSessionSchema(String token){
		return tokenToSession.get(token).schemaName;
	}
	public Long getSessionDatabase(String token){
		return tokenToSession.get(token).dbId;
	}

	public boolean setSessionPHP(String session,String schema){
		
	
		String url = "http://" + host +":8010" + "/-upload/session-create.php";
		URL obj;

		try {
			obj = new URL(url);
			
			HttpURLConnection con;
			try {
				con = (HttpURLConnection) obj.openConnection();
				//add reuqest header
				con.setRequestMethod("POST");
				con.setRequestProperty("User-Agent", USER_AGENT);
				con.setRequestProperty("Accept-Language", "en-US,en;q=0.5");
				String urlParameters ="session=" + session + "&schema=" + schema;
				con.setRequestProperty( "Content-Type", "application/x-www-form-urlencoded"); 
				con.setRequestProperty( "charset", "utf-8");
				con.setRequestProperty( "Content-Length", Integer.toString( urlParameters.getBytes( StandardCharsets.UTF_8 ).length  ));
				con.setUseCaches( false );
			
				
				// Send post request
				con.setDoOutput(true);
				DataOutputStream wr = new DataOutputStream(con.getOutputStream());
				wr.write(urlParameters.getBytes( StandardCharsets.UTF_8 ));// .write(urlParameters.getBytes());
				wr.flush();
				wr.close();
		 
				int responseCode = con.getResponseCode();
				System.out.println("\nSending 'POST' request to URL : " + url);
				System.out.println("Post parameters : " + urlParameters);
				System.out.println("Response Code : " + responseCode);
		 
				BufferedReader in = new BufferedReader(
				        new InputStreamReader(con.getInputStream()));
				String line;
				while((line = in.readLine()) != null){
					System.out.println(line);
				}
				
				in.close();

			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
				return false;
			}
			 
		
		} catch (MalformedURLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return false;
		}


		return true;
	}
	/**
	 * Deprecated we dont need this anymore
	 * @param file
	 * @return
	 */
	public boolean uploadFile(File file){
		
		
		 String server = host;
	        int port = 22;
	        String user = "";
	        String pass = "";
	 
	        FTPClient ftpClient = new FTPClient();
	        try {
	 
	            ftpClient.connect(server, port);
	            ftpClient.login(user, pass);
	            ftpClient.enterLocalPassiveMode();
	 
	            ftpClient.setFileType(FTP.BINARY_FILE_TYPE);
	 
	            String secondRemoteFile = file.getName();
	            InputStream inputStream = new FileInputStream(file);
	 
	            System.out.println("Start uploading second file");
	            OutputStream outputStream = ftpClient.storeFileStream(secondRemoteFile);
	            byte[] bytesIn = new byte[4096];
	            int read = 0;
	 
	            while ((read = inputStream.read(bytesIn)) != -1) {
	                outputStream.write(bytesIn, 0, read);
	            }
	            inputStream.close();
	            outputStream.close();
	 
	            boolean completed = ftpClient.completePendingCommand();
	            if (completed) {
	                System.out.println("The second file is uploaded successfully.");
	            }
	 
	        } catch (IOException ex) {
	            System.out.println("Error: " + ex.getMessage());
	            ex.printStackTrace();
	            return false;
	        } finally {
	            try {
	                if (ftpClient.isConnected()) {
	                    ftpClient.logout();
	                    ftpClient.disconnect();
	                }
	            } catch (IOException ex) {
	                ex.printStackTrace();
	            }
	        }
		return true;
	}
	
	public Connector(String host, Integer port,Integer launcherPort){
		this.port = port;
		this.host = host;
		this.launcherPort = launcherPort;
	}
	
	@PostConstruct
	public void startServer(){
		try{
			//ballsy

//			Runtime.getRuntime().exec("/usr/bin/Simplicity 8890 /disk1//.conf");
//			sendMessage(host, launcherPort, port.toString(),false,"",false);			
		}catch(Exception e){
			System.out.println("Could not start the  Server");
		}

	}
	
	public String getUplopadFolder(){
		try {
			
			if(uploadFolder.equals("")){
				uploadFolder = sendMessage(this.host,this.port,"get upload folders",false,"");
			}
			return uploadFolder; 
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return "";
		}
	}
	
	public boolean checkConnection(){
		
		try {
			return sendMessage(this.host,this.port,"ping",false,"").contains("success");
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return false;
		}
			
	
		
	}
	//send a query that runs outside of a schema, like create schema
	public String sendMessage(String message) throws IOException{
		return sendMessage(this.host,this.port,message,true,"");
	}
	
	
	//sends a query in a schema but without a database, like create database
	public String sendMessage(String message,String schema) throws IOException{
		
		return sendMessage(this.host,this.port,schema + "┌∩┐(◣_◢)┌∩┐" + message,true,"");
	}
	
	//sends a query that runs on a databasae
	public String sendMessage(String message,String schema,String database,String fileName) throws IOException{
		return sendMessage(this.host,this.port,schema + "┌∩┐(◣_◢)┌∩┐" + database + "┌∩┐(◣_◢)┌∩┐" + message,true,fileName);
	}
	

	public String sendMessage(final String host, final Integer port, String message,boolean checkConnection,String fileName) throws IOException{
		return sendMessage(host,port,message,checkConnection,fileName,true);
	}
		
	//send a message on a specific host and port
	public String sendMessage(final String host, final Integer port, String message,boolean checkConnection,String fileName,boolean showOutput) throws IOException{
		if(checkConnection){
			if(! this.checkConnection()){
			     
				startServer();
/*				Runnable r = new Runnable() {
			         public void run() {
			        	 
			            try {
			            		startServer();
						} catch (Exception e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
			         }
			     };

			     new Thread(r).start();
	*/
				if(this.checkConnection()){
					//then connection is good let the rest of th ecode continue
				}else{
					return "The server is restarting please try again in a moment.";	
				}
				
			}
		}
		
        Socket socket = null;
        PrintWriter out = null;
        BufferedReader in = null;
        String response ="";
        try {
            socket = new Socket(host, port);
            out = new PrintWriter(new BufferedWriter(new OutputStreamWriter(socket.getOutputStream(), "UTF-8")), true);
            in = new BufferedReader(new InputStreamReader(socket.getInputStream(),"UTF-8"));
            System.out.println(message);
            //we have to append size of message
            //messageSize:<message.size()>;dataSize:0;<message-here>
            out.println("messageSize:" + message.getBytes("UTF-8").length +";dataSize:0;"+message);
            char[] b = new char[4096 * 1024 * 4];
            Integer size;
            if(fileName.equals("")){
            	if(showOutput){
            		
            		while((size = in.read(b)) != -1){
                		response += new String(b,0,size);		
                    	
                    }
            		if(response == ""){
            			return "The was an error processing your last query";
            		}
            	}
            	
            	
        	}else{
        		if(showOutput){
            		FileOutputStream fos = new FileOutputStream(fileName);
            		Writer output = new OutputStreamWriter(fos, "UTF8");
	          		while((size = in.read(b)) != -1){
	          			output.write(b,0,size);
	                  	
	                }
	          		output.close();
	          		fos.close();        			
        		}

        	}
            
            
        } catch (UnknownHostException e) {
        	e.printStackTrace();
        	System.err.println("Unknown host - " + host);
        	response = "Unknown host - " + host;
        } 
        
        out.close();
        in.close();
        socket.close();
        return response;
	}
	
	// these next two functions allow you to specify a specific host and port in case you ever wanted to 
	public String sendMessage(String host, Integer port, String message,String schema) throws IOException{
		return sendMessage(host,port,schema + "┌∩┐(◣_◢)┌∩┐" + message,true,"");
	}
	
	public String sendMessage(String host, Integer port,String message,String schema,String database,String fileName) throws IOException{
		return sendMessage(host,port,schema + "┌∩┐(◣_◢)┌∩┐" + database + "┌∩┐(◣_◢)┌∩┐" + message,true,fileName);
	}
	public String getDataFolder() {
		try {
			return sendMessage(this.host,this.port,"get data folders",false,"");
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return "";
		}
	}

	public void removeSession(String token) {
		this.tokenToSession.remove(token);
		
	}
	
}
