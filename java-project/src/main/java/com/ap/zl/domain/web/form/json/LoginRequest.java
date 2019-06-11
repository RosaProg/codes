package com...domain.web.form.json;

public class LoginRequest {

	
	private String username;
	private String password;
	private Long tabletId;
	
	public String getUsername() {
		return username;
	}
	
	public void setUsername(String username) {
		this.username = username;
	}
	
	
	public String getPassword() {
		return password;
	}
	
	public void setPassword(String password) {
		this.password = password;
	}
	
	
	public Long getTabletId() {
		return tabletId;
	}
	public void setTabletId(Long tabletId) {
		this.tabletId = tabletId;
	}
	
}
