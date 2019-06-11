package com...repository;

import java.util.List;

import org.springframework.data.repository.CrudRepository;

import com...domain.Users;
/**
 * Repositorio de usuarios. Es lo que interactua con hibernate para generar los queries
 * @author felipe
 *
 */
public interface UsersRepository extends CrudRepository<Users, Long> {
	/**
	 * Regresa el usuario con el nombre dado si es que existe
	 * @param username	nombre de usuario
	 * @return	regresa un uusario
	 */
	Users getUsersByUsername(String username);

	
	List<Users> findByUsername(String name);
/**
 * Regresa un usuario segun su dni
 * @param dni	dni de usuario
 * @return	regresa un usuario
 */

}
