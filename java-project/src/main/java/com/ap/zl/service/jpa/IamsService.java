package com...service.jpa;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.RequestParam;

import com.amazonaws.services.s3.model.Bucket;
import com.amazonaws.services.s3.model.S3ObjectSummary;
import com...domain.BucketDetails;
import com...domain.Iams;
import com...domain.KeyDetails;
import com...domain.Users;
import com...repository.IamsRepository;
import com...service.AwsConnectionService;
import com...util.Connector;
import com...util.EncryptionDecryption;
import com...util.Pbkdf2PasswordEncoder;

@Service("iamsService")
@Transactional
public class IamsService  {
	@Autowired
	private IamsRepository repository;
	
	@Autowired
	Connector ;
	

	HashMap<Long,HashMap<String,AwsConnectionService>> userSessions = new HashMap<Long,HashMap<String,AwsConnectionService>>();

	Pbkdf2PasswordEncoder encoder = new Pbkdf2PasswordEncoder();
	
	public HashMap<Long, HashMap<String, AwsConnectionService>> getUserSessions() {
		return userSessions;
	}

	public void setUserSessions(HashMap<Long, HashMap<String, AwsConnectionService>> userSessions) {
		this.userSessions = userSessions;
	}

	public Iams save(Iams data){
		return repository.save(data);
	}
	
	public List<Iams> findAll(){
	//	return repository.findAllByOrderByNumAsc();
		List<Iams> iamss = (List<Iams>) repository.findAll();
	
		return iamss;
		
	}
	
	public List<Iams> findAll(Long[] list){
		//	return repository.findAllByOrderByNumAsc();
		List<Long> listList = Arrays.asList(list);
		return (List<Iams>) repository.findAll(listList);
			
		}
	
	public void delete(Iams data){
		repository.delete(data);
	}
	
	public void delete(Long id){
		repository.delete(id);
	}

	public Iams findOne(Long id) {
		return repository.findOne(id);
	}

	public List<Iams> save(List<Iams> iams) {
		return (List<Iams>) repository.save(iams);
	}
	
	public boolean registerIamsUser(Users user,String password, String accessKeyId,
			 String secretAccessKey, String iamsUser, Boolean persist) throws Exception {

		
		AwsConnectionService amznService = new AwsConnectionService(accessKeyId,secretAccessKey,);
		if(userSessions.get(user.getId()) == null){
			userSessions.put(user.getId(), new HashMap<String,AwsConnectionService>());
		}
		userSessions.get(user.getId()).put(iamsUser, amznService);

		
		if(persist){

			if(encoder.isPasswordValid(user.getPassword(), password, user.getUsername())){
				Iams iams = new Iams();
				EncryptionDecryption.EncryptedPair accessKeyEncrypted = EncryptionDecryption.encrypt(password, user.getName(), accessKeyId);
				EncryptionDecryption.EncryptedPair secretKeyEncrypted = EncryptionDecryption.encrypt(password, user.getName(), secretAccessKey);
				
				iams.setAccessKeyId(accessKeyEncrypted.cipherText);
				iams.setAccessKeyIdIv(accessKeyEncrypted.iv);
				iams.setSecretAccessKey(secretKeyEncrypted.cipherText);
				iams.setSecretAccessKeyIv(secretKeyEncrypted.iv);
				iams.setUser(user);
				iams.setName(iamsUser);
				iams = save(iams);
				
			}else{
				return false;
			}
			
		}
		return true;
	}

	public List<KeyDetails> getKeys(Users user, String iamsUser, String bucket) {

		AwsConnectionService amznService = userSessions.get(user.getId()).get(iamsUser);
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
		return summaryList;
	}

	public List<String> getIams(Users user) {
		List<String> iamsList = new ArrayList<String>();
		if(userSessions.get(user.getId()) != null){
			for(String iamsUser : userSessions.get(user.getId()).keySet()){
				iamsList.add(iamsUser);
			}			
		}
		return iamsList;
	}
	
	public boolean checkIamsLoaded(Users user){
		for(Iams iams : user.getIams()){
			if(userSessions.get(user.getId()) != null ){
				if(userSessions.get(user.getId()).get(iams.getName()) == null){
					return false;
				}
			}else{
				return false;
			}
		}
		return true;
	}

	public void loadIams(Users user, String password) throws Exception {
		if(! checkIamsLoaded(user)){
			if(encoder.isPasswordValid(user.getPassword(), password, user.getUsername())){
				
				for(Iams iams : user.getIams()){
					String accessKeyId = EncryptionDecryption.decrypt(iams.getAccessKeyId(), password, user.getName(), iams.getAccessKeyIdIv());
					String secretAccessKey = EncryptionDecryption.decrypt(iams.getSecretAccessKey(), password, user.getName(), iams.getSecretAccessKeyIv());
				
					AwsConnectionService amznService = new AwsConnectionService(accessKeyId,secretAccessKey,);
					if(userSessions.get(user.getId()) == null){
						userSessions.put(user.getId(), new HashMap<String,AwsConnectionService>());
					}
					userSessions.get(user.getId()).put(iams.getName(), amznService);

					
				}
			}	
		}
		
	}

	public List<BucketDetails> getBuckets(Users user, String iamsUser) {
		AwsConnectionService amznService = userSessions.get(user.getId()).get(iamsUser);
		List<BucketDetails> summaryList = new ArrayList<BucketDetails>();
		
		List<Bucket> buckets = amznService.getBuckets();
		for(Bucket bucket : buckets){
			BucketDetails detail = new BucketDetails();
			detail.name = bucket.getName();
			detail.createdDate = bucket.getCreationDate();
			detail.owner = bucket.getOwner().getDisplayName();
			
			
			summaryList.add(detail);
		}
		return summaryList;
	}

	public void migrateFile(String iamUser, String bucket, String key, String destination, Users user) {
		// TODO Auto-generated method stub
		 AwsConnectionService amznService = userSessions.get(user.getId()).get(iamUser);
	     amznService.migrateKey(bucket,key,destination,user);
	            
				
	}

}
