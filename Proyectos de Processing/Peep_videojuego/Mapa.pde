class Mapa {
  Nivel1 nivel1;
  Nivel2 nivel2;
  Nivel3 nivel3;
  Nivel4 nivel4;
  ResetArea resetArea;

  int indexCheckPoint;
  ArrayList <CheckPoint> checkPoints = new ArrayList<CheckPoint>();


  Mapa() {
    nivel1 = new Nivel1();
    nivel2 = new Nivel2();
    nivel3 = new Nivel3();
    nivel4 = new Nivel4();
    resetArea = new ResetArea();
    nivel1.Start();
    nivel2.Start();
    nivel3.Start();
    nivel4.Start();
  }

  void DibujarNiveles() {
    nivel1.Update();
    nivel2.Update();
    nivel3.Update();
    nivel4.Update();
  }
}





//-----------------------------------------------------------------------------------------------------------
class Nivel1 {
  Plataforma[] suelo = new Plataforma[8];
  float nivelPosX = unidad*3;
  float nivelPosY = 0;


  Nivel1() {
    float size = 6 * unidad;
    suelo[0] = new Plataforma (nivelPosX + (0 * unidad), nivelPosY + (20 * unidad), size, size);
    suelo[1] = new Plataforma (nivelPosX + (6 * unidad), nivelPosY + (20 * unidad), size, size);
    suelo[2] = new Plataforma (nivelPosX + (12 * unidad), nivelPosY + (20 * unidad), size, size);
    suelo[3] = new Plataforma (nivelPosX + (18 * unidad), nivelPosY + (20 * unidad), size, size);
    suelo[4] = new Plataforma (nivelPosX + (24 * unidad), nivelPosY + (18.5 * unidad), size, size);
    suelo[5] = new Plataforma (nivelPosX + (30 * unidad), nivelPosY + (17 * unidad), size, size); 
    suelo[6] = new Plataforma (nivelPosX + (38 * unidad), nivelPosY + (15 * unidad), size, size); 
    suelo[7] = new Plataforma (nivelPosX + (49 * unidad), nivelPosY + (12 * unidad), size, size);
    println("nivel 1 inicializo");
  }


  void Start() {
    for (int i=0; i < suelo.length; i++) {
      suelo[i].GetIndex(i);
      suelo[i].GetNivel(1);
      suelo[i].DarColor();
      suelo[i].Enumerar();
      suelo[i].StartAreaDeVuelo();
    }
    AddEventosDeCambioDeDireccionDeSprites();
    AddAreaDeCaidaEnLaterales();
  }

  void Update() {   
    for (int i=0; i < suelo.length; i++) {
      suelo[i].Update();
    }
  }

  void AddEventosDeCambioDeDireccionDeSprites() {
    for (int i=0; i < suelo.length-1; i++) {
      mundo.add(new AreaCambiarDireccion(suelo[i], "UP", 1));
    }
    mundo.add(new AreaCambiarDireccion(suelo[7], "UP", -1));
    mundo.add(new AreaCambiarDireccion(suelo[7], "LEFT", -1));
  }

  void AddAreaDeCaidaEnLaterales() {
    suelo[0].StartAreaDeCaida("AMBOS");
    suelo[1].StartAreaDeCaida("AMBOS");
    suelo[2].StartAreaDeCaida("AMBOS");
    suelo[3].StartAreaDeCaida("AMBOS");
    suelo[4].StartAreaDeCaida("RIGHT");
  }
}



//-----------------------------------------------------------------------------------------------------------
class Nivel2 {
  Plataforma[] suelo = new Plataforma[11];
  float nivelPosX = 0;
  float nivelPosY = 0;


  Nivel2() {
    suelo[0] = new Plataforma (nivelPosX + (49 * unidad), nivelPosY + (4 * unidad), 5 * unidad, 5 * unidad);
    suelo[1] = new Plataforma (nivelPosX + (39 * unidad), nivelPosY + (2 * unidad), 5 * unidad, 5 * unidad);
    suelo[2] = new Plataforma (nivelPosX + (36 * unidad), nivelPosY + (1 * unidad), 3 * unidad, 3 * unidad);
    suelo[3] = new Plataforma (nivelPosX + (32 * unidad), nivelPosY + (2 * unidad), 3 * unidad, 3 * unidad);
    suelo[4] = new Plataforma (nivelPosX + (28 * unidad), nivelPosY + (-0.5 * unidad), 4 * unidad, 4 * unidad);
    suelo[5] = new Plataforma (nivelPosX + (22 * unidad), nivelPosY + (-4.5 * unidad), 6 * unidad, 6 * unidad); 
    suelo[6] = new Plataforma (nivelPosX + (29.5 * unidad), nivelPosY + (-9 * unidad), 3 * unidad, 3 * unidad);
    suelo[7] = new Plataforma (nivelPosX + (34 * unidad), nivelPosY + (-10.5 * unidad), 5 * unidad, 5 * unidad); 
    suelo[8] = new Plataforma (nivelPosX + (39 * unidad), nivelPosY + (-10.5 * unidad), 5 * unidad, 5 * unidad);
    suelo[9] = new Plataforma (nivelPosX + (44 * unidad), nivelPosY + (-10.5 * unidad), 5 * unidad, 5 * unidad);
    suelo[10]= new Plataforma (nivelPosX + (34 * unidad), nivelPosY + (-5.5 * unidad), 15 * unidad, 5 * unidad);
    println("nivel 2 inicializo");
  }

  void Start() {
    for (int i=0; i < suelo.length; i++) {
      suelo[i].GetIndex(i);
      suelo[i].GetNivel(2);
      suelo[i].Enumerar();
      suelo[i].DarColor();
      suelo[i].StartAreaDeVuelo();
    }
    suelo[10].areaInteractiva.removeFromWorld();
    suelo[10].checkPoint.removeFromWorld();
    AddEventosDeCambioDeDireccionDeSprites();
    AddAreaDeCaidaEnLaterales();
  }

  void Update() {   
    for (int i=0; i < suelo.length; i++) {
      suelo[i].Update();
    }
  }

  void AddEventosDeCambioDeDireccionDeSprites() {
    mundo.add(new AreaCambiarDireccion(suelo[0], "UP", -1));
    mundo.add(new AreaCambiarDireccion(suelo[0], "RIGHT", -1));
    mundo.add(new AreaCambiarDireccion(suelo[1], "UP", -1));
    mundo.add(new AreaCambiarDireccion(suelo[2], "UP", -1));
    mundo.add(new AreaCambiarDireccion(suelo[3], "UP", -1));
    mundo.add(new AreaCambiarDireccion(suelo[4], "UP", -1));
    mundo.add(new AreaCambiarDireccion(suelo[5], "UP", 1));
    mundo.add(new AreaCambiarDireccion(suelo[5], "RIGHT", 1));

    for (int i=6; i < suelo.length-1; i++) {
      mundo.add(new AreaCambiarDireccion(suelo[i], "UP", 1));
    }
  }

  void AddAreaDeCaidaEnLaterales() {
    suelo[1].StartAreaDeCaida("LEFT");
    suelo[3].StartAreaDeCaida("LEFT");
    suelo[4].StartAreaDeCaida("LEFT");
    suelo[8].StartAreaDeCaida("AMBOS");
  }
}



//-----------------------------------------------------------------------------------------------------------
class Nivel3 {
  Plataforma[] suelo = new Plataforma[4];
  float nivelPosX = 0;
  float nivelPosY = 0;


  Nivel3() {    
    suelo[0] = new Plataforma (nivelPosX + (55 * unidad), nivelPosY + (-12.5 * unidad), 3.5 * unidad, 3.5 * unidad); 
    suelo[1] = new Plataforma (nivelPosX + (58 * unidad), nivelPosY + (-16 * unidad), 3 * unidad, 2.5 * unidad);
    suelo[2] = new Plataforma (nivelPosX + (60 * unidad), nivelPosY + (-19 * unidad), 2.5 * unidad, 2 * unidad);
    suelo[3] = new Plataforma (nivelPosX + (63 * unidad), nivelPosY + (-21.5 * unidad), 4 * unidad, 1.5 * unidad);
    println("nivel 3 inicializo");
  }

  void Start() {
    for (int i=0; i < suelo.length; i++) {
      suelo[i].GetIndex(i);
      suelo[i].GetNivel(3);
      suelo[i].Enumerar();
      suelo[i].DarColor();
      suelo[i].StartAreaDeVuelo();
    }
    AddEventosDeCambioDeDireccionDeSprites();
  }

  void Update() {   
    for (int i=0; i < suelo.length; i++) {
      suelo[i].Update();
    }
  }

  void AddEventosDeCambioDeDireccionDeSprites() {
    mundo.add(new AreaCambiarDireccion(suelo[3], "UP", -1));
    mundo.add(new AreaCambiarDireccion(suelo[3], "LEFT", -1));
  }
}



//-----------------------------------------------------------------------------------------------------------
class Nivel4 {
  Plataforma[] suelo = new Plataforma[6];
  float nivelPosX = 2*unidad;
  float nivelPosY = 2*unidad;

  Nivel4() {
    CrearFamilyBird();
    suelo[0] = new Plataforma (nivelPosX + (50 * unidad), nivelPosY + (-28 * unidad), 5 * unidad, 5 * unidad); 
    suelo[1] = new Plataforma (nivelPosX + (38 * unidad), nivelPosY + (-31 * unidad), 5 * unidad, 5 * unidad);
    suelo[2] = new Plataforma (nivelPosX + (27 * unidad), nivelPosY + (-32 * unidad), 4 * unidad, 4 * unidad);
    suelo[3] = new Plataforma (nivelPosX + (17 * unidad), nivelPosY + (-27 * unidad), 2 * unidad, 2 * unidad);
    suelo[4] = new Plataforma (nivelPosX + (13 * unidad), nivelPosY + (-22 * unidad), 4 * unidad, 4 * unidad);
    suelo[5] = new Plataforma (nivelPosX + (-8 * unidad), nivelPosY + (-23.5 * unidad), 12 * unidad, 12 * unidad);
    println("nivel 4 inicializo");
  }


  void Start() {
    for (int i=0; i < suelo.length; i++) {
      suelo[i].GetIndex(i);
      suelo[i].GetNivel(4);
      suelo[i].Enumerar();
      suelo[i].DarColor();
      suelo[i].StartAreaDeVuelo();
    }
    AddEventosDeCambioDeDireccionDeSprites();
  }


  void Update() {   
    for (int i=0; i < suelo.length; i++) {
      suelo[i].Update();
    }
    if (suelo[5].areaInteractiva.isTouchingBody(bird)) 
      escena = "ganaste";
  }


  void AddEventosDeCambioDeDireccionDeSprites() {
    for (int i=0; i < suelo.length-1; i++) {
      mundo.add(new AreaCambiarDireccion(suelo[i], "UP", -1));
    }
    AreaCambiarDireccion areaExcepcional = new AreaCambiarDireccion(suelo[0], "UP", 1);
    areaExcepcional.adjustPosition(200, 350);
    areaExcepcional.addToWorld(mundo);
  }


  void CrearFamilyBird() {
    FBox familyBird = new FBox(50, 175);
    PImage motherAndBirds = loadImage("motherAndBirds.png");
    float posX = nivelPosX + (-8 * unidad) + (6 * unidad);
    float posY = nivelPosY + (-23.5 * unidad)-(familyBird.getHeight()/2)+10;
    familyBird.setPosition(posX, posY);
    familyBird.attachImage(motherAndBirds);
    familyBird.setSensor(true);
    mundo.add(familyBird);
  }
}//end method
