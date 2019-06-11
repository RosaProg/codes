package com...domain;


import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.Lob;
import javax.persistence.ManyToOne;
import javax.persistence.Table;
import com.fasterxml.jackson.databind.annotation.JsonSerialize;


@JsonSerialize(include=JsonSerialize.Inclusion.NON_EMPTY)
@Entity
@Table(name = "iams")
public class Iams  implements java.io.Serializable, Comparable<Iams>  {

	


	/**
	 * 
	 */
	private static final long serialVersionUID = 5525405283768528695L;

	@Id
	@GeneratedValue
	@Column(name="id")	
	private Long id;
	
	
	@Column(name="username")
	private String name;
	
	@ManyToOne(fetch = FetchType.EAGER)
	@JoinColumn(name = "userId")
	private Users user;

	public Long getId() {
		return id;
	}


	public void setId(Long id) {
		this.id = id;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public Users getUser() {
		return user;
	}

	public void setUser(Users user) {
		this.user = user;
	}
	

	
	@Lob
	@Column(name="accessKeyId")
	private byte[] accessKeyId;
	
	@Lob
	@Column(name="secretAccessKey")
	private byte[] secretAccessKey;
	
	@Lob
	@Column(name="accessKeyIdIv")
	private byte[] accessKeyIdIv;
	
	@Lob
	@Column(name="secretAccessKeyIv")
	private byte[] secretAccessKeyIv;

	public byte[] getAccessKeyId() {
		return accessKeyId;
	}


	public void setAccessKeyId(byte[] accessKeyId) {
		this.accessKeyId = accessKeyId;
	}


	public byte[] getSecretAccessKey() {
		return secretAccessKey;
	}


	public void setSecretAccessKey(byte[] secretAccessKey) {
		this.secretAccessKey = secretAccessKey;
	}


	public byte[] getAccessKeyIdIv() {
		return accessKeyIdIv;
	}


	public void setAccessKeyIdIv(byte[] accessKeyIdIv) {
		this.accessKeyIdIv = accessKeyIdIv;
	}


	public byte[] getSecretAccessKeyIv() {
		return secretAccessKeyIv;
	}


	public void setSecretAccessKeyIv(byte[] secretAccessKeyIv) {
		this.secretAccessKeyIv = secretAccessKeyIv;
	}




	@Override
	public int compareTo(Iams o) {
		if(o.getId() > this.getId()){
			return 1;
		}else if(o.getId() < this.getId()){
			return -1;
		}else{
			return 0;
		}
		
	}
	
	
   
}
