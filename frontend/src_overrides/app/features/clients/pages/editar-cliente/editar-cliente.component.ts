import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { ClientesService } from '../../services/clientes.service';

@Component({
  selector: 'app-editar-cliente',
  templateUrl: './editar-cliente.component.html',
  styleUrls: ['./editar-cliente.component.scss']
})
export class EditarClienteComponent implements OnInit {
  id = 0;
  form = this.fb.group({
    ruc: ['', [Validators.required, Validators.pattern(/^\d{11}$/)]],
    razonSocial: ['', Validators.required],
    telefono: [''],
    correo: ['', Validators.email],
    direccion: ['']
  });

  constructor(private route: ActivatedRoute, private router: Router, private fb: FormBuilder, private api: ClientesService){}

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (this.id && this.id !== 0) {
      this.api.obtener(this.id).subscribe(c => this.form.patchValue(c));
    }
  }

  guardar(): void {
    if (this.form.invalid) return;
    const dto = this.form.value as any;
    if (this.id === 0) {
      this.api.crear(dto).subscribe(() => this.volver());
    } else {
      this.api.actualizar(this.id, dto).subscribe(() => this.volver());
    }
  }

  volver(): void { this.router.navigate(['/clientes']); }
}
