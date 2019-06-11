package com...service.jpa;

import java.util.Arrays;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com...domain.InvitedUser;
import com...repository.InvitedUserRepository;

@Service("invitedUserService")
@Transactional
public class InvitedUserService  {
	@Autowired
	private InvitedUserRepository repository;

	
	
	public InvitedUser save(InvitedUser data){
		return repository.save(data);
	}
	
	public List<InvitedUser> findAll(){
	//	return repository.findAllByOrderByNumAsc();
		List<InvitedUser> invitedUsers = (List<InvitedUser>) repository.findAll();
	
		return invitedUsers;
		
	}
	
	public List<InvitedUser> findAll(Long[] list){
		//	return repository.findAllByOrderByNumAsc();
		List<Long> listList = Arrays.asList(list);
		return (List<InvitedUser>) repository.findAll(listList);
			
		}
	
	public void delete(InvitedUser data){
		repository.delete(data);
	}
	
	public void delete(Long id){
		repository.delete(id);
	}

	public InvitedUser findOne(Long id) {
		return repository.findOne(id);
	}

	public List<InvitedUser> save(List<InvitedUser> invitedUser) {
		return (List<InvitedUser>) repository.save(invitedUser);
	}
	
	

}
