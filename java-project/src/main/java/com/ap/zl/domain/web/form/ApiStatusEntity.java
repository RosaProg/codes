package com...domain.web.form;

/**
 * Clase de utilidad que se usa para responder el status de un pedido via json que incluye un objeto en su respuesta 
 * @author felipe
 *
 * @param <T>
 */
public class ApiStatusEntity<T> 
{
	//------------------------------------------------------------
	// ATTRIBUTES
	//------------------------------------------------------------
	private String status;
	private String errorMsg;
	private T entity;

	//------------------------------------------------------------
	// METHODS
	//------------------------------------------------------------
	public ApiStatusEntity(){
		
	}
	//------------------------------------------------------------
	public ApiStatusEntity(String status, String errorMsg){
		this.status = status;
		this.errorMsg = errorMsg;
	}
	//------------------------------------------------------------
	public String getStatus() {
		return status;
	}
	//------------------------------------------------------------
	public void setStatus(String status) {
		this.status = status;
	}
	//------------------------------------------------------------
	public String getErrorMsg() {
		return errorMsg;
	}
	//------------------------------------------------------------
	public void setErrorMsg(String errorMsg) {
		this.errorMsg = errorMsg;
	}
	//------------------------------------------------------------
	public T getEntity() {
		return entity;
	}
	//------------------------------------------------------------
	public void setEntity(T entity) {
		this.entity = entity;
	}
	//------------------------------------------------------------
}
