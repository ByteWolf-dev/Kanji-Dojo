import { CharacterType } from '../enums/character-type';

export interface BossQuestionDto {
    itemId: number;
    character: string;
    characterType: CharacterType;
}

export interface BossAnswerDto {
    itemId: number;
    answer: string;
}

export interface BossResultDto {
    correct: number;
    total: number;
    passed: boolean;
}
