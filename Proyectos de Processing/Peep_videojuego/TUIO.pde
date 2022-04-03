
//----Todos los eventos TUIO para que no halla errores en Consola----
float rotacionAnterior;
float rotacionActual;
float diferenciaDeRotacion;

int id = 1;
boolean id1Presente = false;


void addTuioObject(TuioObject patron) {
  //Preguntar por la identidad del objeto de TUIO
  if (patron.getSymbolID() == id) {
    //Cargar valores (se encuentran normalizadas)
    id1Presente = true;

    if (patron.getAngleDegrees() < 180)
      rotacionAnterior = patron.getAngleDegrees();
    if (patron.getAngleDegrees() >= 180)
      rotacionAnterior = patron.getAngleDegrees()-360;
  }
}


void updateTuioObject(TuioObject patron)
{

  if (id1Presente == true) {
    if (patron.getAngleDegrees() < 180)
    {
      rotacionAnterior = rotacionActual;
      rotacionActual = patron.getAngleDegrees();
      diferenciaDeRotacion = rotacionActual - rotacionAnterior;
    } else if (patron.getAngleDegrees() >= 180)
    {
      rotacionAnterior = rotacionActual;
      rotacionActual = patron.getAngleDegrees() - 360;
      diferenciaDeRotacion = rotacionActual - rotacionAnterior;
    }

    //rotacionActual = constrain(rotacionActual, -50, 50);//modo1
    rotacionActual = constrain(rotacionActual, -45, 45);//modo1
    //diferenciaDeRotacion = constrain(diferenciaDeRotacion, -25, 25);//modo1
    diferenciaDeRotacion = constrain(diferenciaDeRotacion, -30, 30);//modo1
  }
}


void removeTuioObject(TuioObject tobj) {
  if (tobj.getSymbolID() == id) {
    //Remover fiducial y valores
    id1Presente = false;
  }
}




//------------------------------------------

void addTuioCursor(TuioCursor tcur) {
}

void updateTuioCursor(TuioCursor tcur) {
}

void removeTuioCursor(TuioCursor tcur) {
}

//------------------------------------------

void addTuioBlob(TuioBlob tblb) {
}

void updateTuioBlob(TuioBlob tblb) {
}

void removeTuioBlob(TuioBlob tblb) {
}

//------------------------------------------

void refresh(TuioTime frameTime) {
}
