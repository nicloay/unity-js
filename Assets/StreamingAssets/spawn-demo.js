const engine = require("~engine")

// Our simple serialization/deserialization functions for messages:
const encode = (obj) => JSON.stringify(obj)
const decode = (obj) => JSON.parse(obj)

// Queues for incoming and outgoing messages:
const incoming = []
const outgoing = []

async function sendReceive() {
  // Exchange serialized messages
  var result = await engine.sendMessages(outgoing.map(encode))
  const newIncoming = result.map(decode)
  
  // Place incoming messages in the queue, and clear the outgoing messages we just sent:
  // incoming = incoming.concat(newIncoming)

  incoming.push(...newIncoming);
  outgoing.length = 0;
}

// The known ID of the only Entity in this example:
const cubeId = 1
let rotationX = 0
let scaleY = 0
let isSpaceBarPressed = 0

module.exports.onStart = async function() {
  outgoing.push({ 
    method: "entity_add", 
    data: { id: cubeId }
  })

  outgoing.push({
    method: "entity_transform_update", 
    data: {
      entityId: cubeId,
      transform: {
        position: [0, 0, 0],
        rotation: [0, 0, 0, 0],
        scale: [1, 1, 1]
      }
    }
  })

  await sendReceive()
}

module.exports.onUpdate = async function(dt) {
  
  // Process incoming messages:
  for (msg of incoming) {
    if (msg.method === "key_down" && msg.data.key === "space") {
      isSpaceBarPressed = true
    }
    if (msg.method === "key_up" && msg.data.key === "space") {
      isSpaceBarPressed = false
    }
  }
  // Clear queue
  incoming.length = [];
  /**
   * Pressing the space bar makes the cube grow bigger.
   * If it's released, it shrinks back to its original size.
   */
  if (isSpaceBarPressed) {
    scaleY += dt
  } else {
    scaleY = Math.max(1.0, scaleY - dt)
  }
  /**
   * The cube rotates on the X axis with time
   */
  rotationX += dt

  // Queue outgoing messages:
  outgoing.push({
    method: "entity_transform_update", 
    data: {
      entityId: cubeId,
      transform: {
        position: [0, 0, 0],
        rotation: [Math.sin(rotationX), 0, 0, Math.cos(rotationX)],
        scale: [1, scaleY, 1]
      }
    }
  })

  // Make the exchange:
  await sendReceive()
}
