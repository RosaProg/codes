package com...service.jpa;

import java.util.Arrays;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com...domain.Database;
import com...domain.Users;
import com...repository.DatabaseRepository;

@Service("databaseService")
@Transactional
public class DatabaseService  {
	@Autowired
	private DatabaseRepository repository;

	
	
	public Database save(Database data){
		return repository.save(data);
	}
	
	public List<Database> findAll(){
	//	return repository.findAllByOrderByNumAsc();
		List<Database> databases = (List<Database>) repository.findAll();
	
		return databases;
		
	}
	
	public List<Database> findAll(Long[] list){
		//	return repository.findAllByOrderByNumAsc();
		List<Long> listList = Arrays.asList(list);
		return (List<Database>) repository.findAll(listList);
			
		}
	
	public void delete(Database data){
		repository.delete(data);
	}
	
	public void delete(Long id){
		repository.delete(id);
	}

	public Database findOne(Long id) {
		return repository.findOne(id);
	}

	public List<Database> save(List<Database> database) {
		return (List<Database>) repository.save(database);
	}

	public Database findBySchemaAndName(Users user, String name) {
		// TODO Auto-generated method stub
		return repository.findByUserAndName(user,name);
	}
	
	

}
