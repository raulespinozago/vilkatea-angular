import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { Cliente } from '../models/cliente';

@Injectable({ providedIn: 'root' })
export class ClientesService {
  private base = `${environment.api}/clientes`;
  constructor(private http: HttpClient) {}

  listar(q?: string): Observable<Cliente[]> {
    let params = new HttpParams();
    if (q) params = params.set('q', q);
    return this.http.get<Cliente[]>(this.base, { params });
  }

  obtener(id: number)      { return this.http.get<Cliente>(`${this.base}/${id}`); }
  crear(c: Cliente)        { return this.http.post<Cliente>(this.base, c); }
  actualizar(id: number,c: Cliente){ return this.http.put(`${this.base}/${id}`, c); }
  eliminar(id: number)     { return this.http.delete(`${this.base}/${id}`); }
}
