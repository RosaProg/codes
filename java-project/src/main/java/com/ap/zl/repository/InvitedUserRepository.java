package com...repository;

import java.util.List;

import org.springframework.data.repository.CrudRepository;

import com...domain.InvitedUser;

public interface InvitedUserRepository extends CrudRepository<InvitedUser, Long> {

	List<InvitedUser> findByEmail(String email);

}
