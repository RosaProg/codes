package com...web.controller;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileFilter;
import java.io.FileInputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.Charset;
import java.nio.file.FileSystems;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.nio.file.attribute.BasicFileAttributes;
import java.security.Principal;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collections;
import java.util.Date;
import java.util.HashMap;
import java.util.List;

import javax.annotation.PostConstruct;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.servlet.http.HttpSession;

import org.apache.commons.io.FileUtils;
import org.apache.commons.io.IOUtils;
import org.apache.commons.io.filefilter.AgeFileFilter;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.multipart.MultipartFile;

import com.amazonaws.services.s3.model.Bucket;
import com.amazonaws.services.s3.model.S3ObjectSummary;
import com...domain.BucketDetails;
import com...domain.Database;
import com...domain.Iams;
import com...domain.KeyDetails;
import com...domain.Users;
import com...domain.web.form.ApiStatus;
import com...domain.web.form.ApiStatusEntity;
import com...domain.web.form.ApiStatusGeneric;
import com...service.AwsConnectionService;
import com...service.UsersService;
import com...service.jpa.DatabaseService;
import com...service.jpa.IamsService;
import com...util.Connector;
import com...util.Logger;
import com...util.EncryptionDecryption;
import com...util.Pbkdf2PasswordEncoder;
import com...util.PythonInteractor;
import com...util.RInteractor;
import com...util.ShellInteractor;

import static java.nio.file.StandardCopyOption.*;

/**
 * Handles requests for the application home page.
 */
@Controller
public class AmazonController {

	private static final Logger logger = LoggerFactory.getLogger(AmazonController.class);

	
	@Autowired
	UsersService userService;
	@Autowired
	IamsService iamsService;	

	
	@Autowired
	Logger Logger;
	
	@Autowired
	Connector ;
	
	

	Pbkdf2PasswordEncoder encoder = new Pbkdf2PasswordEncoder();
	
	
	@PostConstruct
	public void init(){
		//load up all of the persisted amazon sessiosn
	}
	
	/**
	 * Initializes an R instance or re;turns the R text
	 * @param principal the user that is ;logged in
	 * @return
	 * @throws IOException
	 */	
	@RequestMapping(value="/amazon/register-iams",method=RequestMethod.POST)
	@ResponseBody
	public ApiStatus registerIams(Principal principal,@RequestParam(value="password",required=false,defaultValue="") String password, @RequestParam("accessKeyId") String accessKeyId,
			@RequestParam("secretAccessKey") String secretAccessKey, @RequestParam("iamsUser") String iamsUser, @RequestParam("persist") Boolean persist){
		ApiStatus response = new ApiStatus();
		
		try{

			Users user = userService.findByUsername(principal.getName());		
			
			if(iamsService.registerIamsUser(user,password, accessKeyId,secretAccessKey,iamsUser, persist)){
				response.setStatus("success");	
			}else{
				response.setStatus("fail");
				response.setErrorMsg("Your password is incorrect.");
					
			}
			
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Could not verify details with amazon.");
			
		}
		return response;
	}

	@RequestMapping(value="/amazon/get-keys",method=RequestMethod.POST)
	@ResponseBody
	public ApiStatusGeneric<KeyDetails> getKeys(Principal principal,@RequestParam("bucket") String bucket,@RequestParam("iamsUser") String iamsUser ){
		ApiStatusGeneric<KeyDetails> response = new ApiStatusGeneric<KeyDetails>();
		
		try{
			Users user = userService.findByUsername(principal.getName());
			
			
			response.setList(iamsService.getKeys(user,iamsUser,bucket));
			response.setStatus("success");
		}catch(Exception e){

			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("failed to get bucket keys for bucket " + bucket + " using the credentials provided.");
		}

		
		
		
		return response;
	}
	@RequestMapping(value="/amazon/migrate-file",method=RequestMethod.POST)
	@ResponseBody
	public ApiStatus migrateFile(Principal principal, @RequestParam("bucket") String bucket,@RequestParam("key") String key,@RequestParam("iamUser") String iamUser,@RequestParam("destination") String destination){
		ApiStatus response = new ApiStatus();
		
		try{
			Users user = userService.findByUsername(principal.getName());
			iamsService.migrateFile(iamUser,bucket,key,destination,user);
			response.setStatus("success");
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Could not migrate file");
		}
		
		return response;
	}
	
	

	@RequestMapping(value="/amazon/get-iams",method=RequestMethod.POST)
	@ResponseBody
	public ApiStatusGeneric<String> getIams(Principal principal){
		ApiStatusGeneric<String> response = new ApiStatusGeneric<String>();
		try{
			
			Users user = userService.findByUsername(principal.getName());
			response.setStatus("success");
			response.setList(iamsService.getIams(user));
		}catch(Exception e){
			e.printStackTrace();
			response.setErrorMsg("Could not list iams users.");
			response.setStatus("fail");
		}
		
		return response;
	}
/*
	@RequestMapping(value="/amazon/check-iams-loaded",method=RequestMethod.POST)
	@ResponseBody
	public ApiStatusEntity<Boolean> checkIamsLoaded(Principal principal){
	ApiStatusEntity<Boolean> response = new ApiStatusEntity<Boolean>();
		
		try{

			Users user = userService.findByUsername(principal.getName());		
			
			response.setEntity(true);
			response.setStatus("success");
			for(Iams iams : user.getIams()){
				if(userSessions.get(user.getId()) != null ){
					if(userSessions.get(user.getId()).get(iams.getName()) == null){
						response.setEntity(false);
					}
				}else{
					response.setEntity(false);
				}
			}
		
		}catch(Exception e){
			e.printStackTrace();
			response.setErrorMsg("Could not check iams");
			response.setStatus("fail");
			
		}
		return response;
		
	}
	*/
	
	@RequestMapping(value="/amazon/load-iams",method=RequestMethod.POST)
	@ResponseBody
	public ApiStatus loadIams(Principal principal,@RequestParam("password") String password){
	ApiStatus response = new ApiStatus();
		
		try{
			
			Users user = userService.findByUsername(principal.getName());		
			if(! iamsService.checkIamsLoaded(user)){
				iamsService.loadIams(user,password);	
			}
			
		}catch(Exception e){
			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("Could not load iams.");
		}
		return response;
	}
	/*
	@RequestMapping(value="/amazon/get-keys-no-store",method=RequestMethod.POST)
	@ResponseBody
	public ApiStatusGeneric<KeyDetails> getKeysNoStore(@RequestParam("accessKeyId") String accessKeyId,
			@RequestParam("secretAccessKey") String secretAccessKey, @RequestParam("bucket") String bucket){
		ApiStatusGeneric<KeyDetails> response = new ApiStatusGeneric<KeyDetails>();
		
		try{
			AwsConnectionService amznService = new AwsConnectionService(acresponse.setStatus("fail");
				response.setErrorMsg("Your password is incorrect.");
				cessKeyId,secretAccessKey);
			List<KeyDetails> summaryList = new ArrayList<KeyDetails>();
			
			List<S3ObjectSummary> summaries = amznService.getBucketKeys(bucket);
			for(S3ObjectSummary summary : summaries){
				KeyDetails detail = new KeyDetails();
				detail.key = summary.getKey();
				detail.lastModified = summary.getLastModified();
				detail.size = summary.getSize();
				detail.owner = summary.getOwner().getDisplayName();
				summaryList.add(detail);
			}
			response.setList(summaryList);
			response.setStatus("success");
		}catch(Exception e){

			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("failed to get bucket keys for bucket " + bucket + " using the credentials provided.");
		}
		
		return response;
	}*/
	
	@RequestMapping(value="/amazon/get-buckets",method=RequestMethod.POST)
	@ResponseBody
	public ApiStatusGeneric<BucketDetails> getBuckets(@RequestParam("iamsUser") String iamsUser, Principal principal){
		ApiStatusGeneric<BucketDetails> response = new ApiStatusGeneric<BucketDetails>();
		
		try{
			Users user = userService.findByUsername(principal.getName());
			
			
			
			response.setList(iamsService.getBuckets(user,iamsUser));
			response.setStatus("success");
		}catch(Exception e){

			e.printStackTrace();
			response.setStatus("fail");
			response.setErrorMsg("failed to list buckets using the credentials provided.");
		}
		
		return response;
	}

	
	
}

