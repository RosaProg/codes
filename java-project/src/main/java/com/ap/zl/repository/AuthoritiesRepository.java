package com...repository;

import java.util.List;

import org.springframework.data.repository.CrudRepository;

import com...domain.Authorities;
import com...domain.Users;
/**
 * Repositorio de autoridades. Es lo que interactua con hibernate para generar los queries
 * @author felipe
 *
 */
public interface AuthoritiesRepository extends CrudRepository<Authorities, Long> {


}
