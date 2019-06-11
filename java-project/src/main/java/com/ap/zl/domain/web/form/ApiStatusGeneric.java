package com...domain.web.form;

import java.util.List;
/**
 * Clase de utilidad que se usa para responder el status de un pedido via json que incluye un lista de objetos en su respuesta 
 * @author felipe
 *
 * @param <T>
 */
import com.fasterxml.jackson.databind.annotation.JsonSerialize;
@JsonSerialize(include=JsonSerialize.Inclusion.NON_EMPTY)
public class ApiStatusGeneric<T> 
{
	//------------------------------------------------------------
	// ATTRIBUTES
	//------------------------------------------------------------
	private String status;
	private String errorMsg;
	private List<T> list;
	private Integer progress;
	//------------------------------------------------------------
	// METHODS
	//------------------------------------------------------------
	public ApiStatusGeneric(){
		
	}
	//------------------------------------------------------------
	public ApiStatusGeneric(String status, String errorMsg){
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
	public List<T> getList() {
		return list;
	}
	//------------------------------------------------------------
	public void setList(List<T> list) {
		this.list = list;
	}
	//------------------------------------------------------------
	public Integer getProgress() {
		return progress;
	}
	public void setProgress(Integer progress) {
		this.progress = progress;
	}
}
