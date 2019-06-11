package com...service.jpa;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com...domain.Settings;
import com...repository.SettingsRepository;
/**
 * Servicio de configuraciones
 * @author felipe
 *
 */
@Service("SettingsService")
@Transactional
public class SettingsService {
	@Autowired
	private SettingsRepository repository;

	public Settings save(Settings data){
		return repository.save(data);
	}
	
	public List<Settings> findAll(){
	//	return repository.findAllByOrderByNumAsc();
		List<Settings> answers = (List<Settings>) repository.findAll();
	
		return answers;
		
	}

	
	public void delete(Settings data){
		repository.delete(data);
	}
	
	public Settings findOne(String key) {
		return repository.findOne(key);
	}

	public List<Settings> save(List<Settings> answers) {
		return (List<Settings>) repository.save(answers);
	}
	
	


}
