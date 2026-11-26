import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';

import { CardService } from '../../services/card';
import { AuthService } from '../../services/auth';
import { CardDto } from '../../dtos/card-dto';

@Component({
    selector: 'app-dashboard',
    imports: [RouterLink, MatButtonModule, MatCardModule, MatToolbarModule],
    templateUrl: './dashboard.html',
    styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {

    private cardService = inject(CardService);
    private authService = inject(AuthService);
    private router = inject(Router);

    cards = signal<CardDto[]>([]);

    ngOnInit(): void {
        this.loadCards();
    }

    loadCards(): void {
        this.cardService.getCardsDueOnOrBefore().subscribe({
            next: (cards) => {
                this.cards.set(cards);
            },
            error: (err) => {
                console.error('Failed to load cards', err);
            }
        });
    }

    logout(): void {
        this.authService.logout().subscribe(() => this.router.navigate(['/login']));
    }
}
