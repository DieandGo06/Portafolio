class Plataforma extends FBox {


  public float[] position = new float [2];
  public CheckPoint checkPoint;
  public FBox areaInteractiva;
  public FBox areaDeVuelo;
  public int index, nivel;
  public FBox[] areaDeCaida = new FBox[2];

  private FBox ancla;
  private FRevoluteJoint union;
  private float tiempoAnimacion;
  private boolean isAnimacionTerminada;
  private String estado;
  private PImage textura;

  public float birdDireccion = 1;
  private boolean conTuio = true;
  private boolean rotacionIgualadaConTuio = false;


  //--------------------------------------------
  Plataforma(float _posX, float _posY, float _sizeX, float _sizeY) {
    super(_sizeX, _sizeY);
    ancla = new FBox(_sizeX*0.2, _sizeY*0.2);
    checkPoint = new CheckPoint(this);

    textura = loadImage("plataformaTexture.png");
    textura.resize(int (getWidth()), int(getHeight()));
    attachImage(textura);

    estado = "noControlable";
    Posicionar(_posX, _posY);
    StartPlataforma();
    StartAncla();
    StartUnion();
    StartAreaInteractiva();
    this.setGroupIndex(-1);//!IMPORTANTE: Este valor al estar negativo, no colisiona con los objetos que tengan este mismo valor en negativo.
  }




  //--------------------------------------------
  void StartPlataforma() {
    this.setName("plataforma");
    this.setStatic(false);
    this.setSensor(false);
    this.setDensity(CalcularDensidad());
    mundo.add(this);
    OcultarEnModoNoDesarrollador(this);
  }
  void StartAncla() {
    ancla.setPosition( position[x], position[y] );
    ancla.setDensity(this.getDensity() * 1000);
    mundo.add(ancla);
    OcultarEnModoNoDesarrollador(ancla);
  }
  void StartUnion() {
    union = new FRevoluteJoint(this, ancla);
    union.setFill(0);
    union.setEnableMotor(true);
    union.setEnableLimit(true);
    union.setCollideConnected(false);
    mundo.add(union);
    if (desarrollador == false) {
      union.setNoFill();
      union.setNoStroke();
    }
  }
  void StartAreaInteractiva() {
    areaInteractiva = new FBox(this.getWidth()-20, 10);
    areaInteractiva.setPosition( position[x], position[y] - (this.getHeight()/2) );
    areaInteractiva.setName("areaNoElegida");
    areaInteractiva.setSensor(true);
    areaInteractiva.setGroupIndex(-1);
    mundo.add(areaInteractiva);
    OcultarEnModoNoDesarrollador(areaInteractiva);
  }
  void StartAreaDeVuelo() {//Se inicia desde el mapa...
    areaDeVuelo = new FBox(this.getWidth(), 30);
    float ajustarAltura = this.getHeight() - (this.getHeight()/2) + 110;
    areaDeVuelo.setPosition(position[x], position[y] - ajustarAltura);
    areaDeVuelo.setName("areaDeVuelo");
    areaDeVuelo.setGroupIndex(-1);
    areaDeVuelo.setSensor(true);
    areaDeVuelo.setNoFill();
    OcultarEnModoNoDesarrollador(areaDeVuelo);

    //... porque necesita el nivel de la plataforma
    if (this.nivel != 3) mundo.add(areaDeVuelo);
  }
  void StartAreaDeCaida(String borde) {//Se inicia desde el mapa...
    for (int i=0; i < areaDeCaida.length; i++) {

      if (borde.equals("RIGHT") && i == 0)//Se detiene el primer ciclo
        continue;
      else if (borde.equals("LEFT") && i == 1)//Se detiene el segundo ciclo
        break;
      else if (borde.equals("AMBOS"));//No se deteniene

      areaDeCaida[i] = new FBox (5, this.getHeight()*0.3);
      areaDeCaida[i].setName("areaDeCaida");
      areaDeCaida[i].setGroupIndex(-1);
      areaDeCaida[i].setSensor(true);
      areaDeCaida[i].setDensity(0.1);
      areaDeCaida[i].setFill(255, 40, 80);
      areaDeCaida[i].setNoStroke();
      if (i == 0) areaDeCaida[i].setPosition(position[x]-(this.getWidth()/2), position[y]);
      if (i == 1) areaDeCaida[i].setPosition(position[x]+(this.getWidth()/2), position[y]);
      mundo.add(areaDeCaida[i]);
      OcultarEnModoNoDesarrollador(areaDeCaida[i]);

      FRevoluteJoint unionAreaDeCaida = new FRevoluteJoint(this, areaDeCaida[i]);
      unionAreaDeCaida.setFill(0);
      unionAreaDeCaida.setEnableMotor(true);
      unionAreaDeCaida.setEnableLimit(true);
      mundo.add(unionAreaDeCaida);
      if (!desarrollador) {
        unionAreaDeCaida.setNoFill();
        unionAreaDeCaida.setNoStroke();
      }
    }//end for
  }



  //--------------------------------------------
  void Update() {
    checkPoint.Update();

    //Cuando la pajaro toca una nueva plataforma
    if (areaInteractiva.isTouchingBody(bird)  &&  areaInteractiva.getName() == "areaNoElegida") {
      //Se desactivan todas y luego se activa la que esta bajo contacto
      DesactivarPlataformas();
      rotacionIgualadaConTuio = false;
      areaInteractiva.setName("areaElegida");
      estado = "play-animation";
    }

    //Para reproducir la animacion de seleccion
    if (estado == "play-animation") {
      AnimacionDeSeleccion("controlable");
    }

    //Plataforma controlable
    if (estado == "controlable"  &&  areaInteractiva.getName() == "areaElegida") {
      bird.GetPlataformaControlable(this);
      IgualarRotacionConTuio();
      Girar();  
      LimitarGiro();
      //println("velocidad angular " + this.getAngularVelocity());
    }

    //Regresa la plataforma a su lugar una vez se elije otra
    if (areaInteractiva.getName() == "areaNoElegida"  &&  estado == "controlable") {
      ReiniciarPlataforma();
      LimitarGiro();
    }

    if (areaInteractiva.getName() == "areaNoElegida"  &&  estado == "noControlable") {
      ReiniciarPlataforma();//Es un doble reinicio para evitar bugs
    }
  }


  //--------------------------------------------
  void Girar() {
    float speed = radians(35);
    //float factor = 4000000;
    //float factor = 800000;
    float factor = 4400000;
    
    
    //float factor = 1800000;---modo1
    

    //Manejo mediante el teclado
    if (keyPressed) {
      if (key == CODED) {
        if (keyCode == RIGHT) ancla.adjustAngularVelocity(speed);
        if (keyCode == LEFT) ancla.adjustAngularVelocity(-speed);
      }
    }

    //Control con Reactivision
    if (conTuio) {
      if (id1Presente) {
        
        //this.setAngularVelocity(getAngularVelocity()*0.7);
        //adjustAngularVelocity(diferenciaDeRotacion/5);
        //this.resetForces();
        
        
        //this.setAngularVelocity(getAngularVelocity()*0.7);//---modo1
        this.setAngularVelocity(getAngularVelocity()*0.7);//---modo1
        
        
        addTorque(diferenciaDeRotacion * factor);


        //println("anterior " + rotacionAnterior);
        //println("actual " + rotacionActual);
        if (diferenciaDeRotacion != 0) {
          //println("rotacion diferencial " + diferenciaDeRotacion);
        }
      }
    }
  }

  //---------------------
  void LimitarGiro() {
    float limite = radians(45);
    if (getRotation() >= limite) {
      setRotation(limite);
      setAngularVelocity(0);
      adjustAngularVelocity(radians(-1));
    } else {
      if (getRotation() <= -limite) {
        setRotation(-limite);
        setAngularVelocity(0);
        adjustAngularVelocity(radians(1));
      }
    }
  }
  
  
  //--------------------
  float CalcularDensidad() {
    //float densidadAbsoluta = 100;
    float densidadAbsoluta = 105;
    float areaCorrelativa = (6*unidad) * (6*unidad);
    float areaDePlataforma = getWidth() * getHeight();
    float factorDensidad = areaCorrelativa/areaDePlataforma;
    float densidadRelativa = densidadAbsoluta * factorDensidad;
    return densidadRelativa;
  }
  

  //---------------------
  void IgualarRotacionConTuio() {
    if (conTuio) 
      if (!rotacionIgualadaConTuio) {
        this.setRotation(radians(rotacionActual));
        rotacionIgualadaConTuio = true;
      }
  }


  //--------------------------------------------
  void DesactivarPlataformas() {
    //Busca a las areas de interaccion de cada plataforma
    ArrayList <FBody> todosLosObjetos = mundo.getBodies();
    for (FBody objeto : todosLosObjetos) {
      if (objeto.getName() == "areaElegida") 
        objeto.setName("areaNoElegida");
    }
  }

  //--------------------------------------------
  void AnimacionDeSeleccion(String _estado) {
    if (isAnimacionTerminada == false) {
      if (tiempoAnimacion == 0) {
        this.addImpulse(0, 7000000);
      }
      if (tiempoAnimacion == 20) {
        this.addImpulse(0, -9000000);
      }
      if (tiempoAnimacion > 30) {
        this.setVelocity(0, 0);
        this.resetForces();
        estado = _estado;
        tiempoAnimacion = 0;
        isAnimacionTerminada = true;
      }
    tiempoAnimacion++;
    }      
    //println(this.getVelocityY());
    //println(ancla.getForceY());
  }




  //----------------------------------------------------------------------------------------
  void ReiniciarPlataforma() {
    setAngularVelocity(0);
    ReiniciarRotacion();
    ReiniciarPosicion();
    ReiniciarEstado();
    ReiniciarAreaInteractiva();
    isAnimacionTerminada = false;
  }
  //------------------------
  void ReiniciarRotacion() {
    if (degrees(getRotation()) >= 1) {
      float _rotacion = degrees(getRotation())-1.5;
      setRotation(radians(_rotacion));
    }
    if (degrees(getRotation()) <= -1) {
      float _rotacion = degrees(getRotation())+1.5;
      setRotation(radians(_rotacion));
    }
  }
  //------------------------
  void ReiniciarPosicion() {
    if (this.getX() <= position[x]  ||  this.getY() <= position[y]) {
      float diferenciaDePosicionX = this.getX() - this.position[x];
      float diferenciaDePosicionY = this.getY() - this.position[y];
      float movimientoX = diferenciaDePosicionX/1.5;
      float movimientoY = diferenciaDePosicionY/1.5;
      setPosition( position[x]-movimientoX, getY()-movimientoY );
    }
  }
  //------------------------
  void ReiniciarEstado() {
    if (degrees(getRotation()) > -0.5  &&  degrees(getRotation()) < 0.5) 
      if (this.getY() > position[y]-0.5  &&  this.getY() < position[y]+0.5) 
        estado = "noControlable";
  }
  //------------------------
  void ReiniciarAreaInteractiva() {
    areaInteractiva.resetForces();
    areaInteractiva.setVelocity(0, 0);
    areaInteractiva.setPosition( position[x], position[y] - (this.getHeight()/2) );
  }
  //----------------------------------------------------------------------------------------






  //--------------------------------------------
  void Posicionar(float _posX, float _posY) {
    position[x] = _posX + RectModeCorner(x);
    position[y] = _posY + RectModeCorner(y);
    setPosition( position[x], position[y] );
  }
  //--------------------
  float RectModeCorner(int eje) {
    if (eje == x) return this.getWidth()/2;
    else return this.getHeight()/2;
  }


  //--------------------------------------------
  void OcultarEnModoNoDesarrollador(FBody _objeto) {
    if (!desarrollador) {
      _objeto.setNoFill();
      _objeto.setNoStroke();
    }
  }

  //--------------------------------------------
  void DarColor() {
    if (nivel == 1) setFill(87, 225, 59);//verde
    else if (nivel == 2) setFill(59, 196, 225);//azul
    else if (nivel == 3) setFill(125, 59, 225);//morado
    else if (nivel == 4) setFill(225, 59, 167);//rosa
    else if (nivel == 5) setFill(225, 99, 59);//naranja
    else setFill(255);
  }


  //--------------------------------------------
  void Enumerar() {
    if (desarrollador) 
    {
      for (int i=0; i < this.index; i++) {
        FBox contador = new FBox(10, 10);
        contador.setPosition(position[x]+(15*i), position[y]-(this.getHeight()/2)+25);
        contador.setSensor(true);
        contador.setGroupIndex(-1);
        mundo.add(contador);
      }
    }
  }
  //-------------------------
  void GetIndex(int _index) {
    index = _index;
  }
  //-------------------------
  void GetNivel(int _nivel) {
    nivel = _nivel;
  }
}
