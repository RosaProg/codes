<?php 
namespace Application\Entity;
use Doctrine\ORM\Mapping as ORM;

/** @ORM\Entity */
class estadoCamara
{
    /**
    * @ORM\Id
    * @ORM\GeneratedValue(strategy="AUTO")
    * @ORM\Column(type="integer")
    */
    private $id;
	/** @ORM\Column(type="string",length=50) */
    private $nombre;
	/** @ORM\Column(type="string",length=250) */
    private $descripcion;
	/** @ORM\Column(type="string",length=250) */
    private $icono;
	/** @ORM\Column(type="string",length=50) */
    private $altausuario;
	/** @ORM\Column(type="date") */
    private $altafecha;
	/** @ORM\Column(type="string",length=50) */
    private $modiusuario;
	/** @ORM\Column(type="date") */
    private $modifecha;
	/** @ORM\Column(type="string") */
    private $bajausuario;
	/** @ORM\Column(type="date") */
    private $bajafecha;
	/** @ORM\Column(type="string",length=3) */
    private $estadoregistro;
	
	public function setId($fn)
    {
        $this->id = $fn;
     
        return $this;
    }
	  public function getId()
    {
        return $this->id;
    }
	public function setNombre($fn)
    {
        $this->nombre = $fn;
     
        return $this;
    }
	  public function getNombre()
    {
        return $this->nombre;
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
	public function setIcono($fn)
    {
        $this->icono = $fn;
        return $this;
    }
	public function getIcono()
    {
        return $this->icono;
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