package com...service;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.ArrayList;
import java.util.List;

import org.springframework.stereotype.Service;

import com.amazonaws.auth.BasicAWSCredentials;
import com.amazonaws.services.s3.AmazonS3;
import com.amazonaws.services.s3.AmazonS3Client;
import com.amazonaws.services.s3.model.Bucket;
import com.amazonaws.services.s3.model.GetObjectRequest;
import com.amazonaws.services.s3.model.ListObjectsRequest;
import com.amazonaws.services.s3.model.ObjectListing;
import com.amazonaws.services.s3.model.S3Object;
import com.amazonaws.services.s3.model.S3ObjectSummary;
import com...domain.Users;
import com...util.Connector;


public class AwsConnectionService {

	BasicAWSCredentials awsCreds;
	AmazonS3 s3Client;
	Connector ;
	public AwsConnectionService(String accessKeyId, String secretAccessKey, Connector ){
		try{
			awsCreds = new BasicAWSCredentials(accessKeyId,secretAccessKey);
			
			s3Client = new AmazonS3Client(awsCreds);
			this. = ;
		}catch(Exception e){
			e.printStackTrace();
		}
		
	}

	public List<Bucket> getBuckets(){
		return s3Client.listBuckets();
	}
	
	public List<S3ObjectSummary> getBucketKeys(String bucket){
		List<S3ObjectSummary> summaries = new ArrayList<S3ObjectSummary>();
		try{

			ListObjectsRequest listObjectsRequest = new ListObjectsRequest().withBucketName(bucket);
			ObjectListing objectList;
			do{
				objectList = s3Client.listObjects(listObjectsRequest);
				for(S3ObjectSummary objectSummary : objectList.getObjectSummaries()){
					summaries.add(objectSummary);
				}
			}while (objectList.isTruncated());
		}catch(Exception e){
			e.printStackTrace();
		}
		return summaries;
	}

	public void migrateKey(String bucket, String key, String destination, Users user) {
		// TODO Auto-generated method stub
		try{
			
			String UploadFolder;
			if(destination.equals("")){
				UploadFolder = .getUplopadFolder() + File.separator + user.getId();
			}else{
				UploadFolder = .getUplopadFolder() + File.separator + user.getId() + File.separator + destination;
			}
			
			File destFile = new File(UploadFolder + File.separator + key);
			
			
			 s3Client.getObject(new GetObjectRequest(bucket, key), destFile);
		        
		       
			
			

		}catch(Exception e){
			e.printStackTrace();
		}
	}

}
