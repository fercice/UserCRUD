import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {Component, Inject} from '@angular/core';
import {DataService} from '../../services/data.service';
import {FormControl, Validators} from '@angular/forms';
import {Usuario} from '../../models/usuario';

@Component({
  selector: 'app-add.dialog',
  templateUrl: '../../dialogs/add/add.dialog.html',
  styleUrls: ['../../dialogs/add/add.dialog.css']
})

export class AddDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: Usuario,
              public dialogRef: MatDialogRef<AddDialogComponent>,            
              public dataService: DataService) {}

  formControl = new FormControl('', [
    Validators.required,
    Validators.email
  ]);

  ngOnInit() {        
    this.dataService.listarEscolaridade().subscribe(res => {
        this.data.listaEscolaridade = res;        
      }
    );
  }

  getErrorMessage() {
    return this.formControl.hasError('required') ? 'Campo obrigatório' :
      this.formControl.hasError('email') ? 'Não é um email válido' :
        '';
  }

  submit() {
  // emppty stuff
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  public confirmAdd(): void {
    this.dataService.adicionarUsuario(this.data, this.dialogRef);
  }
}
