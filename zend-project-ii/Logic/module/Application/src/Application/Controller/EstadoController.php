<?php
/**
 * Zend Framework (http://framework.zend.com/)
 *
 * @link      http://github.com/zendframework/ZendSkeletonApplication for the canonical source repository
 * @copyright Copyright (c) 2005-2015 Zend Technologies USA Inc. (http://www.zend.com)
 * @license   http://framework.zend.com/license/new-bsd New BSD License
 */

namespace Application\Controller;

use Zend\Mvc\Controller\AbstractActionController;
use Zend\View\Model\ViewModel;
use Zend\Db\ResultSet\ResultSet;
use Zend\Db\Adapter\Adapter;
use Zend\Db\Sql\Sql;

class EstadoController extends AbstractActionController
{
 public $dbAdapter;
    public function indexAction()
    {
		$this->dbAdapter=$this->getServiceLocator()->get('Zend\Db\Adapter');
        $estado_camara=$this->dbAdapter->query("select * from estadocamara WHERE estadoregistro='ASC'",Adapter::QUERY_MODE_EXECUTE);
        $estado_camaras=$estado_camara->toArray();
		$del_camara=$this->dbAdapter->query("select * from estadocamara where estadoregistro='DEL'",Adapter::QUERY_MODE_EXECUTE);
        $del_camaras=$del_camara->toArray();

		if(@$_POST['undelete_estado']){
	  $this->undeleteAction(@$_POST['undelete_estado_text']);
	  }
	  if(@$_POST['New_estado']){
		$renderer = $this->serviceLocator->get('Zend\View\Renderer\RendererInterface');
		$url = $renderer->basePath('/application/estado/addEstado');
	   echo "<script>window.location.assign('".$url."');</script>";
	  }
	  if(@$_POST['edit_estado']){
	  $this->editEstadoAction();
	  }
	  
	  if(@$_POST['delete_estado']){
	  $this->deleteEstadoAction(@$_POST['delete_estado_id']);
	  }
	  return array(
		  'estado_camaras'=>$estado_camaras,
		  'undeletelist' =>$del_camaras
	  );
	  
    }
	public function addEstadoAction(){
	$this->dbAdapter=$this->getServiceLocator()->get('Zend\Db\Adapter');
	if(@$_POST['save_estado']){
	$data = array(
    'nombre'      => @$_POST['nombre'],
    'descripcion'      => @$_POST['descripcion'],
    'icono'      => @$_POST['icono'],
    'altausuario'      => @$_POST['altausuario'],
    'altafecha'      => @$_POST['altafecha'],
    'modiusuario'      => @$_POST['modiusuario'],
    'modifecha'      => @$_POST['modifecha'],
    'bajafecha'      => @$_POST['bajafecha'],
    'bajausuario'      => @$_POST['bajausuario'],
    'estadoregistro'      => 'ASC'
);

	$sql = new Sql($this->dbAdapter);
		$insert = $sql->insert()
					->into('estadocamara')
					->columns(array_keys($data))
					->values($data);
					
	$insertString = $sql->getSqlStringForSqlObject($insert);
	
        $result= $this->dbAdapter->query($insertString, Adapter::QUERY_MODE_EXECUTE);
	$renderer = $this->serviceLocator->get('Zend\View\Renderer\RendererInterface');
	$url = $renderer->basePath('/application/estado/index');
	return $this->redirect()->toUrl($url);
	}else{
		
	    $this->dbAdapter=$this->getServiceLocator()->get('Zend\Db\Adapter');
        $sql = new Sql($this->dbAdapter);
        $select = $sql->select()
                      ->from('estadocamara');
        $selectString = $sql->getSqlStringForSqlObject($select);
        $result= $this->dbAdapter->query($selectString, Adapter::QUERY_MODE_EXECUTE);
        $datos=$result->toArray();
		
	return new ViewModel(array("estados"=>$datos));
	}
	}
	
	public function editEstadoAction(){
	$renderer = $this->serviceLocator->get('Zend\View\Renderer\RendererInterface');
	$url = $renderer->basePath('/application/estado/editEstado');
	$url_home = $renderer->basePath('/application/estado/index');
	if(@$_POST['edit_estado_id']){ echo "<script>window.location.assign('".$url."?id=".$_POST['edit_estado_id']."');</script>";}
	
	$id=@$_GET['id'];
	
	$this->dbAdapter=$this->getServiceLocator()->get('Zend\Db\Adapter');
	if(@$_POST['save_edit']){
	$where[] = "id = ".$id;
	$sql = new Sql($this->dbAdapter);
	//echo 'UPDATE `estadocamara` SET nombre= "'.@$_POST['nombre'].'" , descripcion = "'.@$_POST['descripcion'].'",icono="'.@$_POST['icono'].'",altausuario="'.@$_POST['altausuario'].'",altafecha="'.@$_POST['altafecha'].'",modiusuario="'.@$_POST['modiusuario'].'",modifecha="'.@$_POST['modifecha'].'",bajafecha="'.@$_POST['bajafecha'].'",bajausuario="'.@$_POST['bajausuario'].'", estadoregistro="ASC" WHERE id='.$id;
	$result= $this->dbAdapter->query('UPDATE `estadocamara` SET nombre= "'.@$_POST['nombre'].'" , descripcion = "'.@$_POST['descripcion'].'",icono="'.@$_POST['icono'].'",altausuario="'.@$_POST['altausuario'].'",altafecha="'.@$_POST['altafecha'].'",modiusuario="'.@$_POST['modiusuario'].'",modifecha="'.@$_POST['modifecha'].'",bajafecha="'.@$_POST['bajafecha'].'",bajausuario="'.@$_POST['bajausuario'].'", estadoregistro="ASC" WHERE id='.$id, Adapter::QUERY_MODE_EXECUTE);	
	
	return $this->redirect()->toUrl($url_home);
	}else{
	    $this->dbAdapter=$this->getServiceLocator()->get('Zend\Db\Adapter');
        $sql = new Sql($this->dbAdapter);
					$select_est = $sql->select()
                      ->from('estadocamara')
					  ->where('id='.$id);
        $select_est_String = $sql->getSqlStringForSqlObject($select_est);
        $result_est= $this->dbAdapter->query($select_est_String, Adapter::QUERY_MODE_EXECUTE);
        $datos_est=$result_est->toArray();
	}
	 return new ViewModel(array("estado"=>@$datos_est));
	}
	
	public function deleteEstadoAction($id){
	$renderer = $this->serviceLocator->get('Zend\View\Renderer\RendererInterface');
	$url_home = $renderer->basePath('/application/estado/index');
	$sql = new Sql($this->dbAdapter);
		$select_est = $sql->select()
                      ->from('camara')
					  ->where('estado = '.$id);
        $select_est_String = $sql->getSqlStringForSqlObject($select_est);
        $result_est= $this->dbAdapter->query($select_est_String, Adapter::QUERY_MODE_EXECUTE)->toArray();
		if(count($result_est[0])==0){
        $deleteString = 'UPDATE estadocamara SET estadoregistro="DEL" WHERE id ='.$id.'';
        $result= $this->dbAdapter->query($deleteString, Adapter::QUERY_MODE_EXECUTE);
		return $this->redirect()->toUrl($url_home);
		}else{
		echo "<script>alert('No se puede borrar ! Camara id='+'".$result_est[0]['id']."');</script>";
		}
	
	}
	public function undeleteAction($id){
		$renderer = $this->serviceLocator->get('Zend\View\Renderer\RendererInterface');
	$url_home = $renderer->basePath('/application/estado/index');

	if($id!=''){
	
	$sql = new Sql($this->dbAdapter);
		$result= $this->dbAdapter->query('UPDATE `estadocamara` SET estadoregistro="ASC" WHERE id='.$id, Adapter::QUERY_MODE_EXECUTE);	
        /*$deleteString = 'DELETE FROM camara WHERE id ='.$id.'';
        $result= $this->dbAdapter->query($deleteString, Adapter::QUERY_MODE_EXECUTE);*/
		return $this->redirect()->toUrl($url_home);
	}
	}
}
