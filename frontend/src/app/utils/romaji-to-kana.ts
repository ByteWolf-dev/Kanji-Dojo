const VOWELS = 'aeiou';

const TABLE: Record<string, string> = {
    a: 'あ', i: 'い', u: 'う', e: 'え', o: 'お',
    ka: 'か', ki: 'き', ku: 'く', ke: 'け', ko: 'こ',
    ga: 'が', gi: 'ぎ', gu: 'ぐ', ge: 'げ', go: 'ご',
    sa: 'さ', shi: 'し', si: 'し', su: 'す', se: 'せ', so: 'そ',
    za: 'ざ', ji: 'じ', zi: 'じ', zu: 'ず', ze: 'ぜ', zo: 'ぞ',
    ta: 'た', chi: 'ち', ti: 'ち', tsu: 'つ', tu: 'つ', te: 'て', to: 'と',
    da: 'だ', di: 'ぢ', du: 'づ', de: 'で', do: 'ど',
    na: 'な', ni: 'に', nu: 'ぬ', ne: 'ね', no: 'の',
    ha: 'は', hi: 'ひ', fu: 'ふ', hu: 'ふ', he: 'へ', ho: 'ほ',
    ba: 'ば', bi: 'び', bu: 'ぶ', be: 'べ', bo: 'ぼ',
    pa: 'ぱ', pi: 'ぴ', pu: 'ぷ', pe: 'ぺ', po: 'ぽ',
    ma: 'ま', mi: 'み', mu: 'む', me: 'め', mo: 'も',
    ya: 'や', yu: 'ゆ', yo: 'よ',
    ra: 'ら', ri: 'り', ru: 'る', re: 'れ', ro: 'ろ',
    wa: 'わ', wo: 'を',
    kya: 'きゃ', kyu: 'きゅ', kyo: 'きょ',
    gya: 'ぎゃ', gyu: 'ぎゅ', gyo: 'ぎょ',
    sha: 'しゃ', shu: 'しゅ', sho: 'しょ', sya: 'しゃ', syu: 'しゅ', syo: 'しょ',
    ja: 'じゃ', ju: 'じゅ', jo: 'じょ', jya: 'じゃ', jyu: 'じゅ', jyo: 'じょ',
    cha: 'ちゃ', chu: 'ちゅ', cho: 'ちょ', tya: 'ちゃ', tyu: 'ちゅ', tyo: 'ちょ',
    nya: 'にゃ', nyu: 'にゅ', nyo: 'にょ',
    hya: 'ひゃ', hyu: 'ひゅ', hyo: 'ひょ',
    bya: 'びゃ', byu: 'びゅ', byo: 'びょ',
    pya: 'ぴゃ', pyu: 'ぴゅ', pyo: 'ぴょ',
    mya: 'みゃ', myu: 'みゅ', myo: 'みょ',
    rya: 'りゃ', ryu: 'りゅ', ryo: 'りょ',
    xa: 'ぁ', xi: 'ぃ', xu: 'ぅ', xe: 'ぇ', xo: 'ぉ',
    xya: 'ゃ', xyu: 'ゅ', xyo: 'ょ', xtsu: 'っ', xtu: 'っ',
    '-': 'ー'
};

function isVowelOrY(char: string | undefined): boolean {
    return char !== undefined && (VOWELS.includes(char) || char === 'y');
}

function isConsonant(char: string | undefined): boolean {
    return char !== undefined && /[a-z]/.test(char) && !VOWELS.includes(char);
}

export function toHiragana(text: string): string {
    return text.replace(/[ァ-ヶ]/g, char =>
        String.fromCharCode(char.charCodeAt(0) - 0x60)
    );
}

export function toKatakana(text: string): string {
    return text.replace(/[ぁ-ゖ]/g, char =>
        String.fromCharCode(char.charCodeAt(0) + 0x60)
    );
}

// Kana already in the text is kept, so this can run on the whole input value on every keystroke.
// While typing (final = false) a trailing n stays latin because the next key could still make it na/ni/...
export function romajiToKana(input: string, final = false, katakana = false): string {
    const text = input.toLowerCase();
    let result = '';
    let i = 0;

    while (i < text.length) {
        const char = text[i];
        const next = text[i + 1];

        if (char === 'n' && !isVowelOrY(next)) {
            if (next === "'") {
                result += 'ん';
                i += 2;
                continue;
            }

            if (next === 'n') {
                const afterNext = text[i + 2];

                if (isVowelOrY(afterNext)) {
                    // nni -> ん + に
                    result += 'ん';
                    i += 1;
                } else if (afterNext !== undefined || final) {
                    result += 'ん';
                    i += 2;
                } else {
                    result += 'nn';
                    i += 2;
                }
                continue;
            }

            result += next !== undefined || final ? 'ん' : 'n';
            i += 1;
            continue;
        }

        if (isConsonant(char) && (char === next || (char === 't' && text.startsWith('ch', i + 1)))) {
            result += 'っ';
            i += 1;
            continue;
        }

        const match = [4, 3, 2, 1]
            .map(length => text.substring(i, i + length))
            .find(part => TABLE[part] !== undefined);

        if (match) {
            result += TABLE[match];
            i += match.length;
        } else {
            result += char;
            i += 1;
        }
    }

    return katakana ? toKatakana(result) : result;
}
