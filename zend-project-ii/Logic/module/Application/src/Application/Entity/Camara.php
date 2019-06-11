<?php
namespace Application\Entity;
use Doctrine\ORM\Mapping as ORM;
use Doctrine\ORM\EntityRepository;
use Doctrine\Common\Collections\ArrayCollection;
use DoctrineModule\Paginator\Adapter\Collection as Adapter;
use Doctrine\DBAL\Connection;

/** @ORM\Entity */
class Camara 
{
    /**
    * @ORM\Id
    * @ORM\GeneratedValue(strategy="AUTO")
    * @ORM\Column(type="integer")
    */
    private $id;
    /** @ORM\Column(type="string",length=50,nullable=true) */
    private $tipo;
    /** @ORM\Column(type="string",length=50) */
    private $codigo;
	/** @ORM\Column(type="string",length=250) */
    private $descripcion;
    /** @ORM\Column(type="string",length=50) */
    private $posicion;
	/**
     * @ORM\ManyToOne(targetEntity="estadoCamara",cascade={"persist"})
	 * @ORM\JoinColumn(name="estado", referencedColumnName="id")
     **/
    private $estado;
	/** @ORM\Column(type="string",length=15) */
    private $ip;
	/** @ORM\Column(type="string",length=50) */
    private $marca;
	/** @ORM\Column(type="string",length=100) */
    private $modelo;
	/** @ORM\Column(type="string",length=50) */
    private $altausuario;
	/** @ORM\Column(type="date") */
    private $altafecha;
	/** @ORM\Column(type="string",length=50) */
    private $modiusuario;
	/** @ORM\Column(type="date") */
    private $modifecha;
	/** @ORM\Column(type="date") */
    private $bajafecha;
	/** @ORM\Column(type="string") */
    private $bajausuario;
	/** @ORM\Column(type="string",length=3) */
    private $estadoregistro;
    // getters/setters
	public function setId($fn)
    {
        $this->id = $fn;
     
        return $this;
    }
	  public function getId()
    {
        return $this->id;
    }
	
	public function setTipo($fn)
    {
        $this->tipo = $fn;
        return $this;
    }
	public function getTipo()
    {
        return $this->tipo;
    }

	public function setCodigo($fn)
    {
        $this->codigo = $fn;
        return $this;
    }
	public function getCodigo()
    {
        return $this->codigo;
    }

	public function setDescripcion($fn)
    {
        $this->descripcion = $fn;
        return $this;
    }
	public function getDescripcion()
    {
        return $this->descripcion;
    }
	
	public function setPosicion($fn)
    {
        $this->posicion = $fn;
        return $this;
    }
	public function getPosicion()
    {
        return $this->posicion;
    }
	
	public function setEstado($fn)
    {
        $this->estado = $fn;
        return $this;
    }
	public function getEstado()
    {
        return $this->estado;
    }
	
	public function setIp($fn)
    {
        $this->ip = $fn;
        return $this;
    }
	public function getIp()
    {
        return $this->ip;
    }
	
	public function setMarca($fn)
    {
        $this->marca = $fn;
        return $this;
    }
	public function getMarca()
    {
        return $this->marca;
    }
	
	public function setModelo($fn)
    {
        $this->modelo = $fn;
        return $this;
    }
	public function getModelo()
    {
        return $this->modelo;
    }

	public function setAltausuario($fn)
    {
        $this->altausuario = $fn;
        return $this;
    }
	public function getAltausuario()
    {
        return $this->altausuario;
    }
	
	public function setAltafecha($fn)
    {
        $this->altafecha = $fn;
        return $this;
    }
	public function getAltafecha()
    {
        return $this->altafecha;
    }
	
	public function setModiusuario($fn)
    {
        $this->modiusuario = $fn;
        return $this;
    }
	public function getModiusuario()
    {
        return $this->modiusuario;
    }
	
	public function setModifecha($fn)
    {
        $this->modifecha = $fn;
        return $this;
    }
	public function getModifecha()
    {
        return $this->modifecha;
    }
	
	public function setBajausuario($fn)
    {
        $this->bajausuario = $fn;
        return $this;
    }
	public function getBajausuario()
    {
        return $this->bajausuario;
    }
	public function setBajafecha($fn)
    {
        $this->bajafecha = $fn;
        return $this;
    }
	public function getBajafecha()
    {
        return $this->bajafecha;
    }
	public function setEstadoregistro($fn)
    {
        $this->estadoregistro = $fn;
        return $this;
    }
	public function getEstadoregistro()
    {
        return $this->estadoregistro;
}
}

?>