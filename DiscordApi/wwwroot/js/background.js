let documentSettings = {
    width: window.innerWidth,
    height: window.innerHeight,
    points: 256,
    spacing: 50,
    lifetime: 128,
    distance: 10,
    maxdistance: 80,
}

class PNT {
    constructor(x, y) {
        this.x = x
        this.y = y
        this.lifetime = documentSettings.lifetime;
        this.life = 0
        this.fading = true
        this.neighbor = undefined;
        this.checkNeighbors();
    }

    draw() {
        this.updateAlpha();
        strokeWeight(5);
        point(this.x, this.y)
        this.drawNeighbor();
    }

    drawNeighbor() {
        this.updateAlpha();
        strokeWeight(0.1);
        if (this.neighbor != undefined) {
            if (this.neighbor.lifetime > 1) {
                line(this.x, this.y, this.neighbor.x, this.neighbor.y)
            }

        }
    }

    updateAlpha() {
        let alpha;
        if (this.fading) {
            alpha = this.life / documentSettings.lifetime
        }
        else {
            alpha = this.lifetime / documentSettings.lifetime
        }

        stroke('rgba(236,179,101,' + alpha + ')')
        noFill();
    }

    updateStrokeWeight() {
        let alpha;
        if (this.fading) {
            alpha = this.life / documentSettings.lifetime
        }
        else {
            alpha = this.lifetime / documentSettings.lifetime
        }

        strokeWeight(alpha * 5)
    }

    update() {
        if (this.fading) {
            this.life++;
            if (this.life > this.lifetime) {
                this.fading = !this.fading
            }
        }
        else {
            this.lifetime--;
        }

    }

    checkNeighbors() {
        for (let circle = 0; circle < documentSettings.maxdistance; circle++) {
            points.forEach(point => {
                if (calculateDistance(this, point) == circle) {
                    if (point.lifetime >= documentSettings.lifetime * 0.6) {
                        this.neighbor = point
                        return;
                    }

                }
            })
        }

    }
}

let points = []
let lifetime;
let density;
let spacing;
let linkability;


function setup() {
    var cnv = createCanvas(documentSettings.width, documentSettings.height);
    cnv.position(0, 0);
    cnv.style("z-index: -9999;")
}

function draw() {
    background("rgb(4, 28, 50)");
    documentSettings.maxdistance = documentSettings.spacing * Math.sqrt(2) * 1;
    // translate(documentSettings.width / 2, documentSettings.height / 2);
    points.forEach(point => point.draw())
    updatePoints();

}

function updatePoints() {
    points.forEach(
        point => {
            point.update()
            if (point.lifetime < 1) {
                let index = points.indexOf(point)
                points.splice(index, 1)
            }
        })

    if (points.length < documentSettings.points) {
        for (let i = 0; i < 10; i++) {
            points.push(randomPoint());
        }

    }
}

function randomPoint() {
    let x = Math.floor(Math.random() * documentSettings.width / documentSettings.spacing) * documentSettings.spacing;
    let y = Math.floor(Math.random() * documentSettings.height / documentSettings.spacing) * documentSettings.spacing;

    return new PNT(x, y)
}

function calculateDistance(pointA, pointB) {
    var distance = Math.floor(Math.sqrt(Math.pow((pointB.y - pointA.y), 2) + Math.pow((pointB.x - pointA.x), 2)))
    return distance
}