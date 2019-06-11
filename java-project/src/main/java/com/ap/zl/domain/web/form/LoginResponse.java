package com...domain.web.form;

import java.util.ArrayList;
import java.util.List;

import com...domain.Users;

public class LoginResponse extends ApiStatus {
	private Long userId;
	private List<Users> users;
	
	public LoginResponse(){
		setUsers(new ArrayList<Users>());
	}

	public Long getUserId() {
		return userId;
	}

	public void setUserId(Long userId) {
		this.userId = userId;
	}

	public List<Users> getUsers() {
		return users;
	}

	public void setUsers(List<Users> users) {
		this.users = users;
	}
}
