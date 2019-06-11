package com...repository;

import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;

import com...domain.Database;
import com...domain.Users;

public interface DatabaseRepository extends CrudRepository<Database, Long> {

	Database findByUserAndName(Users user, String name);


}
