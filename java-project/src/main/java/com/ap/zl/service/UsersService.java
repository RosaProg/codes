package com...service;


import java.util.List;

import com...domain.Users;
/**
 * Servicio de usuarios. Esta implementado por com...service.jpa.UsersServiceImpl
 * @author felipe
 *
 */
public interface UsersService {
	/**
	 * Regresa un usuario segun el nombre
	 * @param username	nombre de usuario
	 * @return	usuario
	 */
	Users getUsersByUsername(String username);
	/**
	 * guarda un usuario nuevo
	 * @param username	nombre de usuario
	 * @param password	contraseña
	 * @return	el usuario guardado
	 */
	Users save(String username,String password);

	Users findOne(Long userId);

	List<Users> findAll();
/**
 * Agregar un rol a un usuario
 * @param user	usuario
 * @param role	rol para asignar
 */
	void addAuthority(Users user,String role);
	/**
	 * Quitarle un rol a un uusario
	 * @param user	usuario
	 * @param role rol para quitar
	 */
	void deleteAuthority(Users user, String role);
	Users save(Users user);

	void delete(Users user);
	/**
	 * Buscuar usurio por nombre
	 * @param name	nombre de usuario
	 * @return	usuario
	 */
	Users findByUsername(String name);

	String randomString(int len);

	void changePassword(Users user, String pass);

	String resetPassword(String username);

}
