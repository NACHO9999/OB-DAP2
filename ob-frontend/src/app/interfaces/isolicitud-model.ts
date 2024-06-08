import { ICategoriaModel } from "./icategoria-model";
import { IDeptoModel } from "./idepto-model";
import { IUserCreate } from "./user-create";

export interface ISolicitudModel {
    id: string;
    perMan: IUserCreate | null;
    descripcion: string;
    depto: IDeptoModel;
    categoria: ICategoriaModel;
    fechaCreacion: Date;
    fechaCierre: Date | null;
}
