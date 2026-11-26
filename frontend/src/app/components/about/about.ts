import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
    selector: 'app-about',
    imports: [RouterLink, MatButtonModule, MatCardModule],
    templateUrl: './about.html',
    styleUrl: './about.css'
})
export class About {
}
