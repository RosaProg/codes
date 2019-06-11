package com...repository;

import org.springframework.data.repository.CrudRepository;

import com...domain.Settings;

/**
 * Repositorio de configuraciones. Es lo que interactua con hibernate para generar los queries
 * @author felipe
 *
 */
public interface SettingsRepository extends CrudRepository<Settings, String> {


}
