package com...service.jpa;

import java.util.ArrayList;
import java.util.List;
import java.util.Properties;
import java.util.Random;

import javax.mail.Message;
import javax.mail.Multipart;
import javax.mail.PasswordAuthentication;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeBodyPart;
import javax.mail.internet.MimeMessage;
import javax.mail.internet.MimeMultipart;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com...domain.Authorities;
import com...domain.AuthoritiesId;
import com...domain.Users;
import com...repository.AuthoritiesRepository;
import com...repository.UsersRepository;
import com...service.UsersService;
import com...util.Pbkdf2PasswordEncoder;

@Service("usersService")
@Transactional
public class UsersServiceImpl implements UsersService {
	@Autowired
	private UsersRepository usersRepository;

	@Autowired
	private AuthoritiesRepository authoritiesRepository;

	
	@Autowired
	Pbkdf2PasswordEncoder encoder;
	
	@Override
	public Users getUsersByUsername(String username) {
		return usersRepository.getUsersByUsername(username);
	}

	@Override
	public String resetPassword(String username) {
		Users user = this.findByUsername(username);
		String password = randomString(8);
		String encodedPassword = encoder.encodePassword(password, username);
		user.setPassword(encodedPassword);
		this.save(user);
		return password;
	}
	
	
	@Override
	public Users save(String username,String password) {

		/*
		 * 
		 */
		
		final String email = "@db.com";
	    final String emailPass = "terry671";

	    Properties props = new Properties();
	    props.put("mail.smtp.auth", true);
	    props.put("mail.smtp.starttls.enable", true);
	    props.put("mail.smtp.host", "smtp.gmail.com");
	    props.put("mail.smtp.port", "587");

	    Session session = Session.getInstance(props,
	            new javax.mail.Authenticator() {
	                protected PasswordAuthentication getPasswordAuthentication() {
	                    return new PasswordAuthentication(email, emailPass);
	                }
	            });

	    
		String encodedPassword = encoder.encodePassword(password, username);
		Users user = new Users();
		user.setPassword(encodedPassword);
		user.setUsername(username);
		user.setName("");
		user.setEnabled(true);
		user = usersRepository.save(user);
		
		
	    try {

	        Message message = new MimeMessage(session);
	        message.setFrom(new InternetAddress("@db.com"));
	        message.setRecipients(Message.RecipientType.TO,
	                InternetAddress.parse(username));
	        message.setSubject("Welcome to  DB");
	        message.setText(" A user has been created for you in  DB. Please let us know if you have any questions or concerns. /r/r Felipe  (CTO)");
	        
	      
	        
	       


	        Transport.send(message);
	       
	        	
	    } catch (Exception e) {
	        e.printStackTrace();
	       
	    }
	    
		return user;
	}

	@Override
	public Users findOne(Long userId) {
		return usersRepository.findOne(userId);
	}
	
	
	
	@Override
	public List<Users> findAll(){
		return ( List<Users> ) usersRepository.findAll();
	}

	@Override
	public Users save(Users user) {
		return usersRepository.save(user);
	}


	@Override
	public void deleteAuthority(Users user,String role) {
		List<Authorities> toDelete = new ArrayList<Authorities>();
		if(user.getAuthoritieses() != null){
			for(Authorities authority : user.getAuthoritieses())
			{
				if(authority.getId().getAuthority().equals(role)){
					toDelete.add(authority);
				}
			}
			
			authoritiesRepository.delete(toDelete);
				
		}
		
	}

	
	static final String AB = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
	static Random rnd = new Random();

	@Override
	public String randomString( int len ) 
	{
	   StringBuilder sb = new StringBuilder( len );
	   for( int i = 0; i < len; i++ ) 
	      sb.append( AB.charAt( rnd.nextInt(AB.length()) ) );
	   return sb.toString();
	}
	
	@Override
	public void addAuthority(Users user,String role) {
		AuthoritiesId authoritiesId = new AuthoritiesId();
		authoritiesId.setUsername(user.getUsername());
		authoritiesId.setEnabled(true);
		authoritiesId.setAuthority(role);
		Authorities authorities = new Authorities();
		authorities.setUsers(user);
		authorities.setId(authoritiesId);
		
		authoritiesRepository.save(authorities);
		
	}
	@Override
	public Users findByUsername(String name){
		List<Users> users = usersRepository.findByUsername(name);
		if(users.size() > 0 ){
			return users.get(0);			
		}else{
			return null;
		}
		
	}

	@Override
	public void changePassword(Users user, String pass) {

		String encodedPassword = encoder.encodePassword(pass, user.getUsername());
		
		user.setPassword(encodedPassword);
		usersRepository.save(user);
	}

	@Override
	public void delete(Users user) {
		usersRepository.delete(user.getId());
		
	}

}
