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
class IndexController extends AbstractActionController
{
 public $dbAdapter;
    public function indexAction()
    {
		$this->dbAdapter=$this->getServiceLocator()->get('Zend\Db\Adapter');
        $camara=$this->dbAdapter->query("select * from camara",Adapter::QUERY_MODE_EXECUTE);
        $camaras=$camara->toArray();
        $estado_camara=$this->dbAdapter->query("select * from estadocamara",Adapter::QUERY_MODE_EXECUTE);
        $estado_camaras=$estado_camara->toArray();
		
	if(@$_POST['new_camara']){
	  $this->addAction();
	  }
	if(@$_POST['edit']){
	  $this->editAction();
	  }
	  if(@$_POST['delete_id']){
	  $this->deleteAction(@$_POST['delete_id']);
	  }
	  
	  return array(
		  'camaras' => $camaras,
		  'estado_camaras'=>$estado_camaras
	  );
	  
    }
	public function addAction(){
	$this->dbAdapter=$this->getServiceLocator()->get('Zend\Db\Adapter');
	if(@$_POST['save']){
	$data = array(
    'tipo'      => @$_POST['tipo'],
    'codigo'      => @$_POST['codigo'],
    'descripcion'      => @$_POST['descripcion'],
    'posicion'      => @$_POST['posicion'],
    'estado'      => @$_POST['estado'],
    'ip'      => @$_POST['ip'],
    'marca'      => @$_POST['marca'],
    'modelo'      => @$_POST['modelo'],
    'altausuario'      => @$_POST['altausuario'],
    'altafecha'      => @$_POST['altafecha'],
    'modiusuario'      => @$_POST['modiusuario'],
    'modifecha'      => @$_POST['modifecha'],
    'bajafecha'      => @$_POST['bajafecha'],
    'bajausuario'      => @$_POST['bajausuario'],
    'estadoregistro'      => @$_POST['estadoregistro']
);
	$sql = new Sql($this->dbAdapter);
		$insert = $sql->insert()
					->into('camara')
					->columns(array_keys($data))
					->values($data);
					
	$insertString = $sql->getSqlStringForSqlObject($insert);
	echo "<script>alert('".$insertString."');</script>";
        $result= $this->dbAdapter->query($insertString, Adapter::QUERY_MODE_EXECUTE);				
	return $this->redirect()->toRoute('home', array(
    'save' => 'su'));
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
	public function editAction(){
	if(@$_POST['edit_id']){
	 echo "<script>window.location.assign('application/index/edit?id=".$_POST['edit_id']."');</script>";
	}
	$id=@$_GET['id'];
	$this->dbAdapter=$this->getServiceLocator()->get('Zend\Db\Adapter');
	$data = array(
    'ip'      => ''.@$_POST['ip'].'',
    'marca'      => ''.@$_POST['marca'].'',
    'modelo'      => ''.@$_POST['modelo'].'',
    'altausuario'      => ''.@$_POST['altausuario'].'',
    'altafecha'      => ''.@$_POST['altafecha'].'',
    'modiusuario'      => ''.@$_POST['modiusuario'].'',
    'modifecha'      => ''.@$_POST['modifecha'].'',
    'bajafecha'      => ''.@$_POST['bajafecha'].'',
    'bajausuario'      => ''.@$_POST['bajausuario'].'',
    'estadoregistro'      => ''.@$_POST['estadoregistro'].''
);

if(@$_POST['save_edit']){
$where[] = "id = ".$id;
	$sql = new Sql($this->dbAdapter);
	/*$update = $sql->update('bugs', $data, $where);		
	$updateString = $sql->getSqlStringForSqlObject($update);*/
	
$result= $this->dbAdapter->query('UPDATE `camara` SET tipo= "'.@$_POST['tipo'].'" , codigo = "'.@$_POST['codigo'].'", descripcion = "'.@$_POST['descripcion'].'",posicion="'.@$_POST['posicion'].'",estado="'.@$_POST['estado'].'",ip="'.@$_POST['ip'].'",marca="'.@$_POST['marca'].'",modelo="'.@$_POST['modelo'].'",altausuario="'.@$_POST['altausuario'].'",altafecha="'.@$_POST['altafecha'].'",modiusuario="'.@$_POST['modiusuario'].'",modifecha="'.@$_POST['modifecha'].'",bajafecha="'.@$_POST['bajafecha'].'",bajausuario="'.@$_POST['bajausuario'].'",estadoregistro="'.@$_POST['estadoregistro'].'" WHERE id='.$id, Adapter::QUERY_MODE_EXECUTE);	
	
	return $this->redirect()->toRoute('home', array(
    'save' => 'su'));
	}else{
	    $this->dbAdapter=$this->getServiceLocator()->get('Zend\Db\Adapter');
        $sql = new Sql($this->dbAdapter);
        $select = $sql->select()
                      ->from('camara')
					  ->where(array('id'=>$id));
        $selectString = $sql->getSqlStringForSqlObject($select);
        $result= $this->dbAdapter->query($selectString, Adapter::QUERY_MODE_EXECUTE);
        $datos=$result->toArray();
		
		$select_est = $sql->select()
                      ->from('estadocamara');
        $select_est_String = $sql->getSqlStringForSqlObject($select_est);
        $result_est= $this->dbAdapter->query($select_est_String, Adapter::QUERY_MODE_EXECUTE);
        $datos_est=$result_est->toArray();
	}
	 return new ViewModel(array("camara"=>@$datos,"estados"=>@$datos_est));
	}
	public function deleteAction($id){
	$sql = new Sql($this->dbAdapter);
        $deleteString = 'DELETE FROM camara WHERE id ='.$id.'';
        $result= $this->dbAdapter->query($deleteString, Adapter::QUERY_MODE_EXECUTE);
	}
}
