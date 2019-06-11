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
use Doctrine\Common\Collections\ArrayCollection;
use DoctrineModule\Paginator\Adapter\Collection as Adapter;
use Zend\Paginator\Paginator;
use DoctrineORMModule\Paginator\Adapter\DoctrinePaginator;
use Doctrine\ORM\Tools\Pagination\Paginator as ORMPaginator;
use DoctrineModule\Stdlib\Hydrator\DoctrineObject;
use Doctrine\ORM\EntityManager;

class IndexController extends AbstractActionController
{
    public function indexAction()
    {
	$entityManager = $this
						->getServiceLocator()
						->get( 'doctrine.entitymanager.orm_default' );
	
	$dql = "Select c FROM Application\Entity\Camara c where c.estadoregistro = 'ACT'" ;
	$query = $entityManager->createQuery($dql);
	$camaras = $query->getResult();	
	
	if(@$_POST['save']){
	  $this->addAction();
	  }	 
	  return array(
		  'camaras' => $camaras         
	  );
	  
    }
	public function addAction(){
			$entityManager = $this
						->getServiceLocator()
						->get( 'doctrine.entitymanager.orm_default' );
	$dataEstado = new \Application\Entity\EstadoCamara();
    $dataEstado->setId(1);
    $dataEstado->setNombre('');
    $dataEstado->setDescripcion('');
    $dataEstado->setIcono('');
    $dataEstado->setEstadoregistro('');
    $dataEstado->setAltausuario("");
    $dataEstado->setAltafecha(new \DateTime('2015-04-14 00:00:00'));
    $dataEstado->setModiusuario('');
    $dataEstado->setModifecha(new \DateTime('2015-04-14 00:00:00'));
    $dataEstado->setBajausuario('');
    $dataEstado->setBajafecha(new \DateTime('2015-04-14 00:00:00'));
	
	$data = new \Application\Entity\Camara();
    $data->setEstado($dataEstado);
	$data->setCodigo('');
	$data->setDescripcion('');
	$data->setPosicion('');
	 $data->setIp('');
	$data->setMarca('');
	$data->setModelo('');
    $data->setAltausuario("");
	$data->setAltafecha(new \DateTime('2015-04-14 00:00:00'));
    $data->setModiusuario('');
    $data->setModifecha(new \DateTime('2015-04-14 00:00:00'));
    $data->setBajausuario('');
    $data->setBajafecha(new \DateTime('2015-04-14 00:00:00'));
	$data->setEstadoregistro('');
    $entityManager->persist($data);
	$entityManager->flush();
	echo"<script>alert('successfully');</script>";

	}
}
