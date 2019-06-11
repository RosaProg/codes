package com...domain.web.form;
/**
 * Clase de utilidad que se utiliza para regresar el status de un pedido via json
 * @author felipe
 *
 */
public class ApiStatus 
{
	//------------------------------------------------------------
	// ATTRIBUTES
	//------------------------------------------------------------
	private String status;
	private String errorMsg;

	//------------------------------------------------------------
	// METHODS
	//------------------------------------------------------------
	public ApiStatus(){
		
	}
	//------------------------------------------------------------
	public ApiStatus(String status, String errorMsg){
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
}
