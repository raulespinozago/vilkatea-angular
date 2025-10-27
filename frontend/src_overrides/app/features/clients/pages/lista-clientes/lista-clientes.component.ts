import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ClientesService } from '../../services/clientes.service';
import { Cliente } from '../../models/cliente';

@Component({
  selector: 'app-lista-clientes',
  templateUrl: './lista-clientes.component.html',
  styleUrls: ['./lista-clientes.component.scss']
})
export class ListaClientesComponent implements OnInit {
  form = this.fb.group({ q: [''] });
  data: Cliente[] = [];
  cols = ['ruc', 'razonSocial', 'telefono', 'correo', 'direccion', 'acciones'];

  constructor(private fb: FormBuilder, private api: ClientesService, private router: Router){}

  ngOnInit(): void { this.buscar(); }

  buscar(): void {
    const q = this.form.value.q ?? '';
    this.api.listar(q).subscribe(res => this.data = res);
  }

  nuevo(): void { this.router.navigate(['/clientes/editar', 0]); }
  editar(c: Cliente): void { this.router.navigate(['/clientes/editar', c.id]); }

  eliminar(c: Cliente): void {
    if(confirm(`Eliminar a ${c.razonSocial}?`)) {
      this.api.eliminar(c.id!).subscribe(() => this.buscar());
    }
  }
}
