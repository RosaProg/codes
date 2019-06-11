package com...web.controller;

import java.io.BufferedReader;
import java.io.File;
import java.io.InputStreamReader;
import java.math.BigInteger;
import java.security.Principal;
import java.security.SecureRandom;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Properties;

import javax.mail.Message;
import javax.mail.Multipart;
import javax.mail.PasswordAuthentication;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeBodyPart;
import javax.mail.internet.MimeMessage;
import javax.mail.internet.MimeMultipart;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.web.authentication.RememberMeServices;
import org.springframework.security.web.context.SecurityContextRepository;
import org.springframework.stereotype.Controller;
import org.springframework.ui.ModelMap;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import com...domain.Database;
import com...domain.Users;
import com...domain.web.form.ApiStatus;
import com...domain.web.form.ApiStatusEntity;
import com...domain.web.form.ApiStatusGeneric;
import com...service.UsersService;
import com...service.jpa.DatabaseService;
import com...service.jpa.EmailService;
import com...util.Connector;
import com...util.Pbkdf2PasswordEncoder;





@Controller
public class LoginController {


    
    public LoginController(){
    

        
    }
    
	@Autowired
	UsersService userService;
	
	@Autowired
	EmailService emailService;
	
	
	
	@Autowired
	DatabaseService databaseService;
	
	@PersistenceContext
	EntityManager em;
	
	@Autowired
	Connector ;
	
	@Autowired
	Pbkdf2PasswordEncoder encoder;
	
	@RequestMapping(value="/register", method=RequestMethod.GET)
	public String register(ModelMap model)
	{

		
		return "login/register";
	}
	
	@RequestMapping(value="/restorePass", method=RequestMethod.GET)
	public String restore(ModelMap model)
	{

		return "user/restore";
	}
	
	
	@RequestMapping(value="/loginfailed", method=RequestMethod.GET)
	public String loginFailed(ModelMap modelMap)
	{
		modelMap.put("errorMsg", "The login information provided was not correct.");
		
		return "login/login";
	}
	
	@RequestMapping(value="/login", method=RequestMethod.GET)
	public String login(ModelMap modelMap)
	{
		return "login/login";
	}
	
	@RequestMapping(value="/login", method=RequestMethod.POST)
	@ResponseBody
	public String login()
	{
		return "not logged in";
	}
	
	
	@RequestMapping(value="/user/admin", method=RequestMethod.GET)
	public String userAdmin()
	{
		return "home/user_admin";
	}


	@RequestMapping(value="/user/share")
	@ResponseBody
	public ApiStatus addFriend(Principal principal,@RequestParam("username") String username,@RequestParam("databaseId") Long databaseId){
		ApiStatus response = new ApiStatus();
		
		

	    try {

	    	
	    	
	    	
	    	Users user = userService.findByUsername(principal.getName());
			Users friend = userService.findByUsername(username);
			if(friend == null){

		        response.setStatus("fail");
		        response.setErrorMsg("There is not a user with that email registered.");
		        return response;
			}else{
				
			}
			
			user.getFriends().add(friend);
			userService.save(user);

	        response.setStatus("success");
	        	
	    } catch (Exception e) {
	        e.printStackTrace();
	        response.setStatus("fail");
	        response.setErrorMsg("Could not add that friend. Please contact support or try again later.");
	    }
		
		return response;
		
	}
	
	
	@RequestMapping(value="/user/addFriend")
	@ResponseBody
	public ApiStatus addFriend(Principal principal,@RequestParam("username") String username){
		ApiStatus response = new ApiStatus();
		
		

	    try {

	    	Users user = userService.findByUsername(principal.getName());
			Users friend = userService.findByUsername(username);
			if(friend == null){

		        response.setStatus("fail");
		        response.setErrorMsg("There is not a user with that email registered.");
		        return response;
			}
			
			user.getFriends().add(friend);
			userService.save(user);

	        response.setStatus("success");
	        	
	    } catch (Exception e) {
	        e.printStackTrace();
	        response.setStatus("fail");
	        response.setErrorMsg("Could not add that friend. Please contact support or try again later.");
	    }
		
		return response;
		
	}

	@RequestMapping(value="/user/friends")
	@ResponseBody
	public ApiStatusGeneric<Users> listFriends(Principal principal){
		ApiStatusGeneric<Users> response = new ApiStatusGeneric<Users>();
		
		try{
			Users user = userService.findByUsername(principal.getName());
			List<Users> friends = user.getFriends();
			
			for(Users friend : friends){
				em.detach(friend);
				friend.setDatabases(null);
				friend.setFriends(null);
			}
			response.setList(friends);
			response.setStatus("success");
		}catch(Exception e){
	        e.printStackTrace();
	        response.setStatus("fail");
	        response.setErrorMsg("Could not list your friends. Please contact support or try again later.");
		}
		
		return response;
				
	}
	
	
	

	@RequestMapping(value="/user/password")
	@ResponseBody
	public ApiStatus sendPassword(Principal principal,@RequestParam("password") String password){
		ApiStatus response = new ApiStatus();

		

	    try {

			Users user = userService.findByUsername(principal.getName());

	        if(user == null){
	        	response.setStatus("fail");
	        	response.setErrorMsg("This user does not exist");
	        	return response;
	        }

	        userService.changePassword(user,password);


	        response.setStatus("success");
	        	
	    } catch (Exception e) {
	        e.printStackTrace();
	        response.setStatus("fail");
	    }
	    return response;
	}


	@RequestMapping(value="/user/edit", method={ RequestMethod.GET,RequestMethod.POST})
	@ResponseBody
	public ApiStatus editUser(Principal principal, @RequestParam("name") String name){
		ApiStatus status = new ApiStatus();
		try{
			Users user = userService.findByUsername(principal.getName());
			user.setName(name);
			userService.save(user);
			status.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			status.setStatus("fail");
		}
		
		return status;
	}
	
	@RequestMapping(value="/user/save", method={ RequestMethod.GET,RequestMethod.POST})
	public String login(ModelMap model,@RequestParam("username") String username,@RequestParam("name") String name,@RequestParam("password") String password)//,@RequestParam("admin") Boolean admin)
	{
		Users user = null;
		try{
			
			if(userService.findByUsername(username) != null){				
				model.put("username", username);
				model.put("name", name);
				model.put("error", "A user with this username already exists.");
				return "login/register";
				
			}
			user = userService.save(username,password);
			/*if(admin){
				userService.addAuthority(user, "ROLE_ADMIN");
			}*/
			
		

				
				user.setName(name);

				user.setShowTips(true);
				user = userService.save(user);
			
		
				String tempResponse = .sendMessage("create schema " + user.getId().toString());
				if(tempResponse.equals("schema created")){
					userService.addAuthority(user, "ROLE_USER");
					int exitCode = -1;
					String cleanUsername = username.replace(".", "_").replace("@", "_");
					Process p = Runtime.getRuntime().exec("useradd " + cleanUsername);
					
					
					
					exitCode = p.waitFor();

					if(exitCode != 0){
						System.out.println("Code for add user is = " + new Integer(exitCode).toString());
					}
					p = Runtime.getRuntime().exec("echo -e \"" + password + "\n" + password + "\" | passwd " + cleanUsername);
					exitCode = p.waitFor();
					if(exitCode != 0){
						System.out.println("set user passwd is = " + new Integer(exitCode).toString());
					}
					
					p = Runtime.getRuntime().exec("addgroup " + cleanUsername + "_group");
					exitCode = p.waitFor();
					if(exitCode != 0){
						System.out.println("Code for add group is = " + new Integer(exitCode).toString());
					}
					p = Runtime.getRuntime().exec("adduser " + cleanUsername + " " + cleanUsername + "_group");
					exitCode = p.waitFor();
					if(exitCode != 0){
						System.out.println("Code for add user to group is = " + new Integer(exitCode).toString());
					}
					
					String UploadFolder = .getUplopadFolder();
					String Folder = .getDataFolder();
					
					p = Runtime.getRuntime().exec("chown " + cleanUsername + " " + UploadFolder + File.separator + user.getId().toString());
					exitCode = p.waitFor();
					if(exitCode != 0){
						System.out.println("Code for chown user_uploads is = " + new Integer(exitCode).toString());
					}
					p = Runtime.getRuntime().exec("chmod -R 0700 " + UploadFolder + File.separator + user.getId().toString());
					exitCode = p.waitFor();
					if(exitCode != 0){
						System.out.println("Code for chmod user_uploads is = " + new Integer(exitCode).toString());
					}
					
					p = Runtime.getRuntime().exec("chown " + cleanUsername + " " + Folder + File.separator + user.getId().toString());
					exitCode = p.waitFor();
					if(exitCode != 0){
						System.out.println("Code for chown user_data is = " + new Integer(exitCode).toString());
					}
					p = Runtime.getRuntime().exec("chmod -R 0700 " + Folder + File.separator + user.getId().toString());
					exitCode = p.waitFor();
					if(exitCode != 0){
						System.out.println("Code for chmod user_data is = " + new Integer(exitCode).toString());
					}
					
				}else{
					System.out.println(tempResponse);
					model.put("username", username);
					model.put("name", name);
					model.put("error", "We were unable to make the schema for your user. Please wait a few minutes and try again");
					userService.delete(user);
					return "login/register";
				}
				
			
			
		}catch(Exception e){
			e.printStackTrace();
			if(user != null){
				
				userService.deleteAuthority(user, "ROLE_USER");
				userService.delete(user);
			}
		}
		
		return "login/login";
	}
	
	
	@RequestMapping(value="/user/newPassword",method={RequestMethod.GET,RequestMethod.POST})
	@ResponseBody
	public ApiStatus forgotPassword(Principal principal, @RequestParam("newPass") String newPassword , @RequestParam("oldPass") String oldPass){
		ApiStatus status = new ApiStatus();
		try{
			
			Users user = userService.findByUsername(principal.getName());
			
			Pbkdf2PasswordEncoder encoder = new Pbkdf2PasswordEncoder();
			if(encoder.isPasswordValid(user.getPassword(), oldPass, user.getUsername())){
				userService.changePassword(user, newPassword);
				status.setStatus("success");
			}else{
				status.setStatus("fail");
			}
			
			
			  
		}catch(Exception e){
			e.printStackTrace();
			status.setStatus("fail");
		}
		
		return status;

		

		
	}
	
	HashMap<String,String> passwordTokens = new HashMap<String,String>();
	@RequestMapping(value="/user/changePassword",method={RequestMethod.GET,RequestMethod.POST})
	@ResponseBody
	public String forgotPassword(@RequestParam("email") String username,@RequestParam("token") String token,HttpServletRequest request){
		
		try{
			Users user = userService.findByUsername(username);
			
			if(passwordTokens.get(username) != null){
				if(passwordTokens.get(username).equals(token)){
					
					String newPass = userService.resetPassword(username);
			        
			        emailService.sendEmail(username,"We have reset your password for the documentation manager and the new password is " + newPass,"Your password was reset");
					
					return "Your password was changed. Please check your email for the new password.";
							
				}
				return "The link you clicked seems to have expired.";
			}else{
				return "The link you clicked seems to have expired.";
			}
			  
		}catch(Exception e){
			e.printStackTrace();
			return "There was an unspecified error saving your password. Please try again.";
		}
		
		

		

		
	}

	@RequestMapping(value="/user/hasTips",method={RequestMethod.GET,RequestMethod.POST})
	@ResponseBody
	public ApiStatusEntity<Boolean> hasTips(Principal principal){
		ApiStatusEntity<Boolean> response = new ApiStatusEntity<Boolean>();
		try{
			Users user = userService.findByUsername(principal.getName());
			response.setEntity(user.isShowTips());
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Error getting tips.");
		}
		return response;
	}
	
	@RequestMapping(value="/user/hideTips",method={RequestMethod.GET,RequestMethod.POST})
	@ResponseBody
	public ApiStatus hideTips(Principal principal){
		ApiStatus response = new ApiStatus();
		try{
			Users user = userService.findByUsername(principal.getName());
			user.setShowTips(false);
			
			userService.save(user);
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Error hiding tips.");
		}
		return response;
	}

	
	@RequestMapping(value="/user/forgotPassword",method={RequestMethod.GET,RequestMethod.POST})
	@ResponseBody
	public ApiStatus forgotPassword(@RequestParam("email") String username,HttpServletRequest request){
		ApiStatus response = new ApiStatus();
		Users user = userService.findByUsername(username);
		if(user != null){
		
			try{

				String link = request.getRequestURL().toString();
				link = link.substring(0,link.lastIndexOf("/"));
				SecureRandom random = new SecureRandom();

				
				 String token = new BigInteger(260, random).toString(32);
				 passwordTokens.put(username, token);
				 
				 
				//add reset of link here which includes the data needed to reset t he password
				link += "/changePassword?email=" + username +"&token=" + token;
				
			  emailService.sendEmail(username,"Please click on the following link to reset your password for  DB. \r " + link,"Reset your password");
				
				response.setStatus("success");
			}catch (Exception e){
				e.printStackTrace();
				response.setStatus("fail");
				response.setErrorMsg("Error sending email. Please try again later.");
			}
		}else{
			response.setStatus("fail");
			response.setErrorMsg("There is no user with this email address.");
		}
		return response;
	}
	
	
	
	@RequestMapping(value="/views/loginfailed", method = RequestMethod.GET)
	public String loginerror(ModelMap model) {
 
		model.addAttribute("error", "true");
		return "login/login";
 
	}
 
	@RequestMapping(value="/user/archive", method ={ RequestMethod.GET,RequestMethod.POST})
	@ResponseBody
	public ApiStatus archiveUser(@RequestParam("id") Long id){
		ApiStatus response= new ApiStatus();
		
		try{
			Users user = userService.findOne(id);
			user.setEnabled(false);
			userService.save(user);
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("No se pudo archivar usuario.");
		}
		return response;
	}
	
	@RequestMapping(value="/user/list", method ={ RequestMethod.GET,RequestMethod.POST})
	@ResponseBody
	public ApiStatusGeneric<Users> getUsers(){
		ApiStatusGeneric<Users> response = new ApiStatusGeneric<Users>();
		
		try{
			response.setStatus("success");
			response.setList(userService.findAll());
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Error haciendo lista de usuarios.");
		}
		return response;
	} 
	
	
	@RequestMapping(value="/ui/logout", method = RequestMethod.GET)
	public String logout(ModelMap model) {
 
		return "login/login";
 
	}
}
