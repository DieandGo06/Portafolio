class Bird extends FBox {

  String estado;
  float tiempoEnVuelo;
  float [] checkPoint = new float [2];
  PImage[] birdBolitaDer = new PImage[8];
  PImage[] birdBolitaIzq = new PImage[8];
  PImage birdStopDer, birdStopIzq;

  float size = 25;
  int densidad = 10;
  int gravedad = 6000;
  int direccionDePie = 1;

  boolean primerContacto;
  Plataforma plataformaControlable;
  ArrayList <ParticulasPolvo> particulas = new ArrayList <ParticulasPolvo>();

  AudioSample caida;
  ParticulaCanto canto;



  //--------------------------------------------
  Bird() {
    super(25, 25);
    estado = "volando";

    //Cargar Imagenes
    birdStopDer = loadImage("birdStopDer.png");
    birdStopIzq = loadImage("birdStopIzq.png");
    for (int i=0; i<birdBolitaDer.length; i++) {
      birdBolitaDer[i] = loadImage(dataPath("girar-derecha") + "//" + "birdBall0" + i + "Der.png");
      birdBolitaIzq[i] = loadImage(dataPath("girar-izquierda") + "//" + "birdBall0" + i + "Izq.png");
    }
    attachImage(birdBolitaDer[0]);//Su primera imagen en dibujar

    //Propiedades Fisicas 
    setDensity(densidad);
    setFriction(3);
    setDamping(0.4);
    setAngularDamping(5);
    setBullet(true);//mejora el movimiento

    //Sonido
    canto = new ParticulaCanto();
    caida = minim.loadSample(dataPath("sonidos") + "//" + "caida.wav", 512);

    //Propiedades Meta
    setName("bird");
    setFill(255, 0, 0);
    setPosition(width/2, 400);
    mundo.add(this);
  }



  //--------------------------------------------
  void Update() {
    if (estado != null) {
      Gravedad();
      Mover();
      DesvanecerParticulas();
      //println(plataformaControlable);
    }

    if (estado == "parado") 
    {
      if (primerContacto) {
        CrearParticulas();
        PlayGolpeDeCaida();
        this.setRotation(0);
        SeleccionarSpriteDetenido();
        primerContacto = false;
      }
      //Puede cantar de pie o volando pero solo inicia a cantar de pie
      StartSinging();
      Sing();

      //Este contador evita que se cambie el estado por errores de colisiones de la libreria
      //Es un cambio de estado secundario, el pricipal se da en las area de vuelo.
      if (tiempoEnVuelo >= 30) estado = "volando";
    }

    if (estado == "volando") {
      primerContacto = true;
      Animacion();
      Sing();
    }

    if (estado == "respawn")
    {
      Reiniciar();
      plataformaControlable.setAngularVelocity(0);
      plataformaControlable.rotacionIgualadaConTuio = false;
      plataformaControlable.IgualarRotacionConTuio();
      estado = "volando";
    }

    if (EstaTocandoLaPlataforma()) {
      tiempoEnVuelo++;
    }
  }


  //--------------------------------------------------- COLISIONES ---------------------------------------------------

  void ColisionResetArea(FBody other) {
    if (other.getName().equals("reset")) this.estado = "respawn";
  }

  void ColisionAreaDeVuelo(FBody other) {
    if (other.getName().equals("areaDeVuelo")) this.estado = "volando";
  }

  void ColisionAreaCambiarDireccion(FBody other) {
    if (other.getName().equals("cambiarDireccion_derecha")) {
      this.direccionDePie = 1;
      this.setSensor(false);
    }
    if (other.getName().equals("cambiarDireccion_izquierda")) {
      this.direccionDePie = -1;
      this.setSensor(false);
    }
  }

  void ColisionAreaDeCaida(FBody other) {
    if (other.getName().equals("areaDeCaida")) 
      this.setSensor(true);//Se vuelve un trigger
  }

  void ColisionCheckPoint(FBody other) {
    if (other.getName() == "checkPoint") {
      checkPoint[x] = other.getX();
      checkPoint[y] = other.getY();
    }
  }

  void ColisionAreaInteractivaDePlataformas(FBody other) {
    if (other.getName() == "areaNoElegida") {
      other.addForce(0, 40000);
      this.setSensor(false);
    }
  }

  void ColisionPlataforma(FContact colision, FBody other) {
    if (other.getName() == "plataforma") 
      if (colision.getNormalY() > 1) {
        this.estado = "parado";
        tiempoEnVuelo = 0;
      }
  }

  //--------------------------------------------------- END COLISIONES ---------------------------------------------------



  //--------------------------------------------
  void Animacion() {
    int cambiarSprite = frameCount/4;
    if (getAngularVelocity() <= 0) {
      attachImage(birdBolitaDer[cambiarSprite%birdBolitaDer.length]);
    }
    if (getAngularVelocity() > 0) {
      attachImage(birdBolitaIzq[cambiarSprite%birdBolitaIzq.length]);
    }
    //println(direccion);
    //rintln(getAngularVelocity());
  }
  //---------------------
  void SeleccionarSpriteDetenido() {
    if (direccionDePie == 1) attachImage(birdStopDer);
    else if (direccionDePie == -1) attachImage(birdStopIzq);
  }


  //--------------------------------------------
  void GetPlataformaControlable(Plataforma _plataforma) {
    plataformaControlable = _plataforma;
  }

  //--------------------------------------------
  boolean EstaTocandoLaPlataforma() {
    if (!isTouchingBody(plataformaControlable)) return true;
    else return false;
  }

  //--------------------------------------------
  void CrearParticulas() {
    particulas.add(new ParticulasPolvo("RIGHT"));
    particulas.add(new ParticulasPolvo("LEFT"));
    mundo.add(particulas.get(0));
    mundo.add(particulas.get(1));
  }

  //--------------------------------------------
  void DesvanecerParticulas() {
    for (ParticulasPolvo particula : particulas) {
      particula.Desvanecer_Eliminar();
    }
  }

  //--------------------------------------------
  void StartSinging() {
    canto.ReestablecerCanto();
  }

  //--------------------------------------------
  void Sing() {
    canto.PlayAnimacion();
    canto.PlayAudio();
  }

  //--------------------------------------------
  void PlayGolpeDeCaida() {
    caida.trigger();
  }

  //--------------------------------------------
  void Gravedad() {
    addForce(0, gravedad);
    //println(getY());
  }

  //--------------------------------------------
  void Reiniciar() {
    setRotation(0);
    setVelocity(0, 0);
    setPosition(checkPoint[x], checkPoint[y]-100);
  }


  //--------------------------------------------
  void Mover() {    
    if (keyPressed) {
      /*
      if (key == 'a') addForce(-8000, 0);
      if (key == 'd') addForce(8000, 0);
      if (key == 'w') addForce(0, -14000);
      if (key == 's') addForce(0, 8000);
      */
      
      //Inmovilizar
      if (key == 'z') setDensity(0);
      if (key == 'x') setDensity(1);
      
      //Teletrasnportar
      if (key == '2') setPosition(mapa.nivel2.suelo[0].getX(), mapa.nivel2.suelo[0].getY() - 150);
      if (key == '3') setPosition(mapa.nivel3.suelo[0].getX(), mapa.nivel3.suelo[0].getY() - 100);
      if (key == '4') setPosition(mapa.nivel4.suelo[0].getX(), mapa.nivel4.suelo[0].getY() - 150);
      if (key == '5') setPosition(mapa.nivel4.suelo[4].getX(), mapa.nivel4.suelo[4].getY() - 150);
    }
  }
}










//----------------------------------------------------------------------------------------
class ParticulasPolvo extends FBox {

  int speed = 1500;
  float tiempo;
  boolean isLocated = false;

  ParticulasPolvo(String direccion) {
    super(15, 15);
    setSensor(true);
    setNoStroke();
    setFill(244, 235, 217);

    Ubicar();
    Move(direccion);
    setName("polvo");
    mundo.add(this);
  }

  void Move(String direccion) {
    if (!isLocated) {
      if (direccion == "RIGHT") addImpulse(speed, -speed);
      if (direccion == "LEFT") addImpulse(-speed, -speed);
      isLocated = true;
    }
  }

  void Ubicar() {
    setPosition(bird.getX(), bird.getY()+(bird.getHeight())/2);
  }
  void Desvanecer() {
    setFill(244, 235, 217, 100-(tiempo*2.5));
  }
  void Desvanecer_Eliminar() {
    if (tiempo > 30) mundo.remove(this);
    Desvanecer();
    tiempo++;
  }
}









//----------------------------------------------------------------------------------------
class ParticulaCanto {
  FCircle audioPlayer;
  PImage[] particulas = new PImage[10];
  AudioSample[] audio = new AudioSample[3];

  int animationSprite;
  int contadorDeRepeticionesAnimacion = 100;//"=100" es usado para que no inicie el juego con la animacion
  int cantidadDeRepeticionesAnimacion;

  int audioSeleccionado;
  boolean isSinging = true;//Se reproduce el audio en false

  float tiempoTranscurrido;
  int cooldown = 10;


  ParticulaCanto() {
    //Cargar animacion
    for (int i=0; i<particulas.length; i++) {
      particulas[i] = loadImage(dataPath("particulas de canto") + "//" + "canto0" + i + ".png");
    }
    //Cargar sonidos
    for (int i=0; i<audio.length; i++) {
      audio[i] = minim.loadSample(dataPath("sonidos") + "//" + "sing0" + i + ".wav", 512);
    }
    //Propeidades
    audioPlayer = new FCircle(5);
    audioPlayer.attachImage(particulas[0]);
    audioPlayer.setSensor(true);
    audioPlayer.setNoStroke();
    audioPlayer.setNoFill();
    mundo.add(audioPlayer);
  }


  void PlayAnimacion() {
    if (contadorDeRepeticionesAnimacion < cantidadDeRepeticionesAnimacion  &&  isSinging == true) 
    {
      if (frameCount % 4 == 0) {
        audioPlayer.attachImage(particulas[animationSprite]);
        audioPlayer.setPosition(bird.getX(), bird.getY());
        animationSprite++;

        if (animationSprite == particulas.length) {
          animationSprite = 0;
          contadorDeRepeticionesAnimacion++;
          if (contadorDeRepeticionesAnimacion == cantidadDeRepeticionesAnimacion)
            audioPlayer.dettachImage();
        }
      }
    }//end main if
  }//end method


  void PlayAudio() {
    if (isSinging == false) {
      audio[audioSeleccionado].trigger();
      isSinging = true;
    }
  }//end method


  void ReestablecerCanto() {
    if (tiempoTranscurrido > cooldown*60) {
      contadorDeRepeticionesAnimacion = 0;
      tiempoTranscurrido = 0;
      audioSeleccionado++;
      isSinging = false;
    }
    tiempoTranscurrido++;
    setDuracionDeAnimacion();
  }


  void setDuracionDeAnimacion() {
    //Los audios no duran lo mismo. Segun su duracion se repite mas o menos veces la aniamcion
    if (audioSeleccionado == 0) cantidadDeRepeticionesAnimacion = 4;
    else if (audioSeleccionado == 1) cantidadDeRepeticionesAnimacion = 2;
    else if (audioSeleccionado == 2) cantidadDeRepeticionesAnimacion = 3;

    if (audioSeleccionado == audio.length) audioSeleccionado = 0;
  }
}//end class
