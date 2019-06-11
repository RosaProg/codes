package com...util;

import java.math.BigInteger;
import java.security.NoSuchAlgorithmException;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.KeySpec;


import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;

import org.springframework.security.authentication.encoding.PasswordEncoder;
import org.springframework.stereotype.Service;
/**
 * Clase que maneja la encripcion de las contraseñas utilizando pbkdf2. Es mucho mas seguro que los defaults que tiene spring entonces
 * hicimos un encoder de contraseña usando este estandard
 * @author felipe
 *
 */
public class Pbkdf2PasswordEncoder implements  PasswordEncoder{

	@Override
	/**	encripta una constrasena
	 *	@return	la contrasena encriptada
	 */
	public String encodePassword(String rawPass, Object salt) {
		System.out.println("here");
		String saltString = (String) salt;
		byte[] saltBytes = saltString.getBytes();
		KeySpec spec = new PBEKeySpec(rawPass.toCharArray(), saltBytes, 65536, 128);
		SecretKeyFactory f;
		try {
			f = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1");
			byte[] hash;
			try {
				hash = f.generateSecret(spec).getEncoded();
				System.out.println("salt: " + new BigInteger(1, saltBytes).toString(16));
				System.out.println("hash: " + new BigInteger(1, hash).toString(16));
				
				return new BigInteger(1, hash).toString(16);
			} catch (InvalidKeySpecException e) {
			
				e.printStackTrace();
			}

			
		} catch (NoSuchAlgorithmException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		
		return null;
	}

	@Override
	/**
	 * avisa si la contrasena es valida 
	 */
	public boolean isPasswordValid(String encPass, String rawPass, Object salt) {
	
	
		if(encodePassword(rawPass,salt).equals(encPass)){
			return true;
		}else{
			return false;
		}
	}

}
