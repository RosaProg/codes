package com...service.jpa;

import java.util.Properties;

import javax.mail.Message;
import javax.mail.MessagingException;
import javax.mail.PasswordAuthentication;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.AddressException;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;

import org.springframework.stereotype.Service;

@Service("emailService")
public class EmailService {
	final String email = "@db.com";
    final String emailPass = "terry671";

    Properties props = new Properties();

    public EmailService(){
    	props.put("mail.smtp.auth", true);
        props.put("mail.smtp.starttls.enable", true);
        props.put("mail.smtp.host", "smtp.gmail.com");
        props.put("mail.smtp.port", "587");
        
    }
    
    
    public void sendEmail(String recipient, String messageText, String subject){
    	  Session session = Session.getInstance(props,
		            new javax.mail.Authenticator() {
		                protected PasswordAuthentication getPasswordAuthentication() {
		                    return new PasswordAuthentication(email, emailPass);
		                }
		            });
	        
	
	        try {
	            Message message = new MimeMessage(session);
	        	message.setFrom(new InternetAddress(email));
				message.setRecipients(Message.RecipientType.TO,
		                InternetAddress.parse(recipient));
		        message.setSubject(subject);
		        message.setContent(message, "text/html; charset=utf-8");
		       


		        Transport.send(message);
			} catch (AddressException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (MessagingException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
	        
			
    }
}
