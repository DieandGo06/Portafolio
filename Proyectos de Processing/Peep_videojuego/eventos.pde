//----------------------------------------------------------------------------------------  
class PantallaDeVictoria {
  float [] position = new float[2];
  long frameDeVictoria;

  PantallaDeVictoria () {
    pushStyle();
    fill(40,160);
    rect(0,0, width, height);
    textSize(80);
    textAlign(CENTER);
    fill(255, 255, 0);//Amarillo
    text("GANASTE", width/2, height/2);
    popStyle();
    frameDeVictoria = frameCount;
  }

  
}





//----------------------------------------------------------------------------------------  
class ResetArea extends FBox {
  float [] position = new float[2];

  ResetArea () {
    super(6000, 50);
    mundo.add(this);
    setSensor(true);
    setName("reset");
    Posicionar(500, 1150);
    if (desarrollador == false) {
      this.setNoFill();
      this.setNoStroke();
    }
  }

  void Posicionar(float _posX, float _posY) {
    position[x] = _posX;
    position[y] = _posY;
    setPosition( position[x], position[y] );
  }
}





//----------------------------------------------------------------------------------------  
class AreaCambiarDireccion extends FBox {
  float [] position = new float[2];
  String ubicarEnBorde;


  //----------------------
  AreaCambiarDireccion (FBox _plataforma, String borde, int direccion) {
    super(50, 50);
    setSensor(true);
    setFill(180, 240, 60);
    setGroupIndex(-1);

    ubicarEnBorde = borde;
    Dimensionar(borde, _plataforma);
    Posicionar(_plataforma);
    Nombrar(direccion);
    mundo.add(this);
    
    if (desarrollador == false) {
      this.setNoFill();
      this.setNoStroke();
    }
  }
  
  //----------------------
  void Posicionar(FBox _plataforma) {  
    float ajustarX;
    float ajustarY;

    if (ubicarEnBorde == "LEFT") {
      int orientacion = -1;
      ajustarX = ((_plataforma.getWidth()/2) + (this.getWidth()/2) + 20) * orientacion;
      ajustarY = (_plataforma.getHeight()/2) + (this.getHeight()/2);
    } else if (ubicarEnBorde == "RIGHT") {
      int orientacion = 1;
      ajustarX = ((_plataforma.getWidth()/2) + (this.getWidth()/2) + 20) * orientacion;
      ajustarY = (_plataforma.getHeight()/2) + (this.getHeight()/2);
    } else if (ubicarEnBorde == "UP") {
      ajustarX = 0;
      ajustarY = (_plataforma.getHeight()/2) + (this.getHeight()/2) + 100;
    } else {
      ajustarX = 0;
      ajustarY = 0;
      println("estas pasando mal la ubicacion de una AreaCambiarDireccion, solo se permite RIGHT, LEFT y UP");
    }

    //Ubicar Area
    position[x] = _plataforma.getX() + ajustarX;
    position[y] = _plataforma.getY() - ajustarY;
    setPosition( position[x], position[y] );
  }

  //----------------------
  void Dimensionar(String _borde, FBox _plataforma) {
    if (_borde == "LEFT" || _borde == "RIGHT") {
      this.setWidth(10);
      this.setHeight(150);
    } else if (_borde == "UP") {
      this.setWidth(_plataforma.getWidth());
      this.setHeight(10);
    }
  }
  
  //----------------------
  void Nombrar(int _direccion) {
    if (_direccion == 1) setName("cambiarDireccion_derecha");
    if (_direccion == -1) setName("cambiarDireccion_izquierda");
  }
}





//---------------------------------------------------------------------------------------- 
class CheckPoint extends FBox {
  float [] position = { 0, 0 };
  boolean isLocated = false;
  int numDeEsteCheckPoint;
  Plataforma plataforma;


  CheckPoint (Plataforma _plataforma) {
    super(100, 200);
    this.setSensor(true);
    setName("checkPoint");
    setFill(255, 255, 40);
    mundo.add(this);
    plataforma = _plataforma;
    this.setGroupIndex(-1);//!IMPORTANTE: Este valor al estar negativo, no colisiona con los objetos que tengan este mismo valor en negativo.
    
    if (desarrollador == false) {
      this.setNoFill();
      this.setNoStroke();
    }
  }


  //--------------------------------------------
  void Update() {
    if (isLocated == false) 
    {
      Dimensionar();
      Posicionar();
      ContarEsteCheckPoint();
      isLocated = true;
    }
    
    if (isTouchingBody(bird)) 
    {
      EliminarCheckPointsAnteriores();
      mundo.remove(this);
    }
  }

  //--------------------------------------------
  void Posicionar() {
    float posX = Esquina1(plataforma, x);
    float posY = Esquina1(plataforma, y) - this.getHeight();
    this.setPosition(posX, posY);
  }

  //---------------------
  float Esquina1(FBox _plataforma, int eje) {
    if (eje == x) {
      return _plataforma.getX();
    } else {
      return _plataforma.getY() - ( (_plataforma.getHeight() - this.getHeight()) /2 );
    }
  }

  //--------------------------------------------
  void Dimensionar() {
    float sizeX = plataforma.getWidth() - (plataforma.getWidth() * 0.1);
    float sizeY = 20;
    this.setWidth(sizeX);
    this.setHeight(sizeY);
  }


  //--------------------------------------------
  void ContarEsteCheckPoint() {
    numDeEsteCheckPoint = mapa.indexCheckPoint;
    mapa.checkPoints.add(this);
    mapa.indexCheckPoint++;
  }

  //--------------------------------------------
  void EliminarCheckPointsAnteriores() {
    if (modoAsistido)
      for (CheckPoint item : mapa.checkPoints) 
        if (item.numDeEsteCheckPoint < this.numDeEsteCheckPoint)
          mundo.remove(item);
  }
}//end Class
