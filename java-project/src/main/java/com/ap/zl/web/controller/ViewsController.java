package com...web.controller;

import java.io.File;
import java.io.IOException;
import java.security.Principal;

import javax.servlet.http.HttpServletRequest;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Controller;
import org.springframework.ui.ModelMap;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.context.request.RequestContextHolder;

import com...domain.Users;
import com...service.UsersService;
import com...service.jpa.IamsService;
import com...util.Connector;

/**
 * Handles requests for the application home page.
 */
@Controller
public class ViewsController {

	@Autowired
	UsersService service;
	
	@Autowired 
	Connector ;

	@Autowired
	IamsService iamsService;
	
	@RequestMapping(value="/")
	public String mainViewRoot(ModelMap model,Principal principal){
		if(principal == null || principal.getName() == null){
			return "login/login";
		}
		Users user = service.findByUsername(principal.getName());
		
		
		try {
			iamsService.loadIams(user, (String) SecurityContextHolder.getContext().getAuthentication().getCredentials());
			createLinuxUser(user,(String) SecurityContextHolder.getContext().getAuthentication().getCredentials());
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		model.addAttribute("fullName", user.getName());
		return "views/clean";
	
	}
	
	@RequestMapping(value="/editProfile", method=RequestMethod.GET)
	public String editProfile(ModelMap model,Principal principal)
	{
		Users user = service.findByUsername(principal.getName());
		
		model.addAttribute("fullName", user.getName());
		model.addAttribute("email",user.getUsername());
		
		return "user/editProfile";
	}
	
	@RequestMapping(value="/views/main")
	public String mainView(ModelMap model,Principal principal){
		Users user = service.findByUsername(principal.getName());
		
		model.addAttribute("fullName", user.getName());
		

		try {
			iamsService.loadIams(user, (String) SecurityContextHolder.getContext().getAuthentication().getCredentials());
			createLinuxUser(user,(String) SecurityContextHolder.getContext().getAuthentication().getCredentials());
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return "views/clean";
	}
	
	@RequestMapping(value="/views/upload")
	public String uploadView(ModelMap model,Principal principal){
		//.setSessionPHP(RequestContextHolder.currentRequestAttributes().getSessionId(), service.findByUsername(principal.getName()).getId().toString());
		//model.addAttribute("session", RequestContextHolder.currentRequestAttributes().getSessionId());
		return "views/upload";
	}
	
	@RequestMapping(value="/views/editor")
	public String editorView(){
		return "views/editor";
	}
	
	@RequestMapping(value="/views/imageViewer")
	public String imageViewer(){
		return "views/imageViewer";
	}

	@RequestMapping(value="/views/logs")
	public String logs(){
		return "views/logs";
	}

	
	@RequestMapping(value="/views/default")
	public String defaultView(ModelMap model,Principal principal){
		//.setSessionPHP(RequestContextHolder.currentRequestAttributes().getSessionId(), service.findByUsername(principal.getName()).getId().toString());
		Users user = service.findByUsername(principal.getName());

		try {
			iamsService.loadIams(user, (String) SecurityContextHolder.getContext().getAuthentication().getCredentials());
			createLinuxUser(user,(String) SecurityContextHolder.getContext().getAuthentication().getCredentials());
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		model.addAttribute("fullName", user.getName());
		return "views/clean";
	}

	//this deals with issues migrating form server to server
	private void createLinuxUser(Users user,String password) throws IOException, InterruptedException{
		int exitCode = -1;
		String cleanUsername = user.getUsername().replace(".", "_").replace("@", "_");
		Process p = Runtime.getRuntime().exec("useradd " + cleanUsername);
		
		
		
		exitCode = p.waitFor();

		if(exitCode != 0){
			return; //useradd will fail if the user already exists so we return here
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
	}
	
}

