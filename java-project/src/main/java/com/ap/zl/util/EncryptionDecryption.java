package com...util;

import java.security.AlgorithmParameters;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.spec.KeySpec;
import java.util.ArrayList;
import java.util.List;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.spec.SecretKeySpec;
import javax.xml.bind.DatatypeConverter;

public class EncryptionDecryption {




    private static byte[] generateSalt(String username){
    	byte[] salt = username.getBytes();
    	int end = 8;
    	if (salt.length < 8){
    		end = salt.length;
    	}
    	byte[] temp = {0,0,0,0,0,0,0,0};
		for(int i = 0; i < end; i++){
			temp[i] = salt[i];
		}
		salt = temp;
		return salt;
    }

    public static class EncryptedPair{
    	public byte[] iv;
    	public byte[] cipherText;
    }
    
    public static EncryptedPair encrypt(String password, String username, String textToEncrypt) throws Exception {

    	byte[] salt = generateSalt(username);
    	
    	SecretKeyFactory factory = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1");
    	KeySpec spec = new PBEKeySpec(password.toCharArray(), salt, 65536, 128);
    	SecretKey tmp = factory.generateSecret(spec);
    	SecretKey secret = new SecretKeySpec(tmp.getEncoded(), "AES");
    	/* Encrypt the message. */
    	Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
    	cipher.init(Cipher.ENCRYPT_MODE, secret);
    	AlgorithmParameters params = cipher.getParameters();
    	byte[] iv = params.getParameterSpec(IvParameterSpec.class).getIV();
    	byte[] cipherText = cipher.doFinal(textToEncrypt.getBytes("UTF-8"));
    	EncryptedPair returnValue = new EncryptedPair();
    	returnValue.iv = iv;
    	returnValue.cipherText = cipherText;
    	return returnValue;
    	
    }

    public static String decrypt(byte[] cipherText, String password, String username, byte[] iv) throws Exception {

    	byte[] salt = generateSalt(username);
    	
    	SecretKeyFactory factory = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1");
    	KeySpec spec = new PBEKeySpec(password.toCharArray(), salt, 65536, 128);
    	SecretKey tmp = factory.generateSecret(spec);
    	SecretKey secret = new SecretKeySpec(tmp.getEncoded(), "AES");
    	
    	Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
    	cipher.init(Cipher.DECRYPT_MODE, secret, new IvParameterSpec(iv));
    	String plainText = new String(cipher.doFinal(cipherText), "UTF-8");
    	return plainText;
    }


}