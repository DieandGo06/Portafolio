class Camara {
  String estado;
  Grilla grilla;

  Camara() {
    grilla = new Grilla();
  }
}


//-------------------------------------------------------
class Grilla {
  float bordeSize = 30;
  float espaciado = unidad;


  void Update() {
    if (desarrollador) {
      Bordes();
      Vertical();
      Horizontal();
      EsquinaBorde();
    }
  }


  void Bordes() {
    pushStyle();
    fill(80);
    noStroke();
    rect(0, 0, width, bordeSize);
    rect(0, 0, bordeSize, height);
    popStyle();
  }

  void EsquinaBorde() {
    pushStyle();
    noStroke();
    fill(80, 50, 50);
    rect(0, 0, bordeSize, bordeSize);
    popStyle();
  }


  void Vertical() {
    pushMatrix();
    translate(transX, 0);
    pushStyle();
    stroke(0, 70);
    textSize(14);
    fill(220);
    for (int i=-50; i < 100; i+=1) {
      float posX = i * espaciado;
      float posY = 0;
      line(posX, posY, posX, height-posY);
      text(i, posX-(10), posY+(18));
    }
    popStyle();
    popMatrix();
  }


  void Horizontal() {
    pushMatrix();
    translate(0, transY);
    pushStyle();
    stroke(0, 70);
    textSize(14);
    fill(220);
    for (int i=-50; i < 100; i+=1) {
      float posX = 0;
      float posY = i * espaciado;
      line(posX, posY, width-posX, posY);
      text(i, posX+2, posY+4);
    }
    popStyle();
    popMatrix();
  }
}//end class
