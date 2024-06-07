import { IDuenoModel } from "./idueno-model";

export interface IDeptoModel {
    piso: number;
    numero: number;
    dueno: IDuenoModel | null;
    cantidadCuartos: number;
    cantidadBanos: number;
    conTerraza: boolean;
    edificioNombre: string;
    edificioDireccion: string;
}
